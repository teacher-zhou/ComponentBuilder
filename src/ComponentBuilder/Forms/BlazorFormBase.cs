namespace ComponentBuilder.Forms;

/// <summary>
/// Represents a base class to create a &lt;form> element. 
/// </summary>
/// <typeparam name="TFormComponent">The form component type that has almost similar with <see cref="EditForm"/> component.</typeparam>
[HtmlTag("form")]
public abstract class BlazorFormBase<TFormComponent> : BlazorChildContentComponentBase<EditContext>
    where TFormComponent : ComponentBase
{

    private EditContext _fixedEditContext;
    private readonly Func<Task> _handleSubmitDelegate;
    private bool _hasSetEditContextExplicitly;
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorFormBase{TForm}"/> class.
    /// </summary>
    public BlazorFormBase()
    {
        _handleSubmitDelegate = Submit;

    }
    /// <summary>
    ///  Specifies the top-level model object for the form. An edit context will be constructed  for this model. If using this parameter, do not also supply a value for <see cref="EditContext"/>.
    /// </summary>
    [Parameter] public object Model { get; set; }
    /// <summary>
    /// Supplies the edit context explicitly. If using this parameter, do not also supply 
    /// <see cref="Model"/>, since the model value will be taken from the <see cref="EditContext.Model"/> property.
    /// </summary>
    [Parameter]
    public EditContext EditContext
    {
        get => _fixedEditContext;
        set
        {
            _fixedEditContext = value;
            _hasSetEditContextExplicitly = (value != null);
        }
    }

    /// <summary>
    /// The callback that will be invoked when the form is submitted. 
    /// If you use this parameter, it is your responsibility to trigger any validation manually，For example, by calling
    /// <see cref="EditContext.Validate"/> method.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
    /// <summary>
    ///  A callback that will be invoked when the form is submitted and the <see cref="EditContext"/> is determined to be valid.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <summary>
    /// A callback that will be invoked when the form is submitted and the <see cref="EditContext"/> is determined to be invalid.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// Gets a value of statement indicating whether this form is submitting.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this form is submitting; otherwise, <c>false</c>.
    /// </value>
    public bool IsSubmitting { get; private set; }

    protected sealed override string TagName => "form";
    protected override int RegionSequence => _fixedEditContext.GetHashCode();

    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// when supplying a {nameof(OnSubmit)} parameter to {nameof(Form)}, do not also supply {nameof(OnValidSubmit)} or {nameof(OnInvalidSubmit)}.
    /// </exception>
    protected override void OnParametersSet()
    {
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

    protected override EditContext GetChildContentValue() => _fixedEditContext;


    protected override void AddContent(RenderTreeBuilder builder, int sequence)    
    {
        this.CreateCascadingComponent<TFormComponent>(builder, sequence + 2, BuildEditContextCascadingValue, isFixed: true);
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onsubmit"] = _handleSubmitDelegate;
    }


    private void BuildEditContextCascadingValue(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(_fixedEditContext, 0, content => base.AddContent(content, 0), isFixed: true);
    }

    /// <summary>
    /// Submit this form.
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
        await NotifyStateChanged();
    }
}
