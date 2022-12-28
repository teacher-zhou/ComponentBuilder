namespace ComponentBuilder;

/// <summary>
/// Representing a base class to build form component. This is an abstract class.
/// <para>
/// This could be the parent component with casecading parameter for child component.
/// </para>
/// <para>
/// This is same as <see cref="EditForm"/> component to use. 
/// </para>
/// </summary>
/// <typeparam name="TFormComponent">The type of form component.</typeparam>
[HtmlTag("form")]
[ParentComponent]
[Obsolete("The class will be removed in next version. Please Implement from IHasForm interface for form component")]
public abstract class BlazorFormComponentBase<TFormComponent> : BlazorComponentBase, IHasChildContent<EditContext>
    where TFormComponent : ComponentBase
{

    private EditContext _fixedEditContext;
    private readonly Func<Task> _handleSubmitDelegate;
    private bool _hasSetEditContextExplicitly;
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorFormComponentBase{TFormComponent}"/> class.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public BlazorFormComponentBase() => _handleSubmitDelegate = Submit;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    /// <summary>
    /// Specifies the top-level model object for the form. An editing context will be constructed for the model. If this parameter is used, do not provide <see cref="EditContext"/> value.
    /// </summary>
    [Parameter] public object Model { get; set; }
    /// <summary>
    /// Explicitly provide editing context. If this parameter is used, do not provide <see cref="Model"/>; Because the model values will be taken from <see cref="EditContext.Model"/> Properties.
    /// </summary>
    [Parameter]
    public EditContext EditContext
    {
        get => _fixedEditContext;
        set
        {
            _fixedEditContext = value;
            _hasSetEditContextExplicitly = value != null;
        }
    }

    /// <summary>
    /// The callback that will be invoked when the form is submitted. If this parameter is used, it is up to you to trigger any validation manually, for example, by calling <see cref="EditContext.Validate"/> method.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
    /// <summary>
    ///  The callback function that will be called when the form is submitted, and then <see cref="EditContext"/> is judged to be valid.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <summary>
    ///  The callback function that will be called when the form is submitted, and then <see cref="EditContext"/> is judged to be invalid.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// The contents of the form.
    /// </summary>
    [Parameter] public RenderFragment<EditContext>? ChildContent { get; set; }

    /// <summary>
    /// Gets a Boolean value indicating whether the form is in the submitting state.
    /// </summary>
    public bool IsSubmitting { get; private set; }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override int RegionSequence => _fixedEditContext.GetHashCode();

    /// <summary>
    /// Called when the component receives a parameter from the parent component in the rendering tree and the incoming value has been assigned to the property.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// When providing {nameof(OnSubmit)} arguments for {nameof(Form)}, do not provide {nameof(OnValidSubmit)} or {nameof(OnInvalidSubmit)} arguments.
    /// </exception>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (_hasSetEditContextExplicitly && Model != null)
        {
            throw new InvalidOperationException($"{GetType().Name} required a {nameof(Model)} " +
                $"paremeter, or a {nameof(EditContext)} parameter, but not both.");
        }
        else if (!_hasSetEditContextExplicitly && Model == null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires either a {nameof(Model)} parameter, or an {nameof(EditContext)} parameter, please provide one of these.");
        }

        if (OnSubmit.HasDelegate && (OnValidSubmit.HasDelegate || OnInvalidSubmit.HasDelegate))
        {
            throw new InvalidOperationException($"when supplying a {nameof(OnSubmit)} parameter to {GetType().Name}, do not also supply {nameof(OnValidSubmit)} or {nameof(OnInvalidSubmit)}.");
        }

        // Update _editContext if we don't have one yet, or if they are supplying a
        // potentially new EditContext, or if they are supplying a different Model
        if (Model != null && Model != _fixedEditContext?.Model)
        {
            _fixedEditContext = new EditContext(Model!);
        }
    }

    /// <summary>
    /// Create cascading component with <see cref="EditContext"/> object.
    /// </summary>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {

        builder.CreateCascadingComponent(_fixedEditContext, 0, content => content.AddContent(0, ChildContent?.Invoke(_fixedEditContext)), isFixed: true);
    }

    /// <summary>
    /// Build 'onsubmit' callback event attribute with form.
    /// </summary>
    /// <param name="attributes"></param>
    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onsubmit"] = _handleSubmitDelegate;
    }

    /// <summary>
    /// Perform the form submission.
    /// </summary>
    public virtual async Task Submit()
    {
        IsSubmitting = true;
        if (OnSubmit.HasDelegate)
        {
            await OnSubmit.InvokeAsync(_fixedEditContext);
            return;
        }

        bool isValid = _fixedEditContext.Validate();
        if (isValid && OnValidSubmit.HasDelegate)
        {
            await OnValidSubmit.InvokeAsync(_fixedEditContext);
        }

        if (!isValid && OnInvalidSubmit.HasDelegate)
        {
            await OnInvalidSubmit.InvokeAsync(_fixedEditContext);
        }
        IsSubmitting = false;
    }
}
