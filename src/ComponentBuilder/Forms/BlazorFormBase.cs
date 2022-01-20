namespace ComponentBuilder.Forms;

/// <summary>
/// Represents a form base class to create form component that has same action with <see cref="EditForm"/> component.
/// </summary>
/// <typeparam name="TForm">The type of form.</typeparam>
public abstract class BlazorFormBase<TForm> : BlazorChildContentComponentBase<EditContext>
    where TForm : ComponentBase
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

    ///// <summary>
    ///// Renders the component to the supplied <see cref="T:Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder" />.
    ///// </summary>
    ///// <param name="builder">A <see cref="T:Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder" /> that will receive the render output.</param>
    //protected override void BuildRenderTree(RenderTreeBuilder builder)
    //{
    //    builder.OpenRegion(_fixedEditContext.GetHashCode());

    //    builder.OpenElement(0, TagName ?? "form");
    //    BuildComponentAttributes(builder, out int sequence);
    //    builder.AddAttribute(sequence + 1, "onsubmit", _handleSubmitDelegate);
    //    this.CreateCascadingComponent<TForm>(builder, sequence + 2, BuildEditContextCascadingValue, isFixed: true);
    //    builder.CloseElement();

    //    builder.CloseRegion();
    //}

    protected override EditContext GetChildContentValue() => _fixedEditContext;

    protected override void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        base.BuildComponentAttributes(builder, out sequence);
        builder.AddAttribute(sequence + 1, "onsubmit", _handleSubmitDelegate);
        this.CreateCascadingComponent<TForm>(builder, sequence + 2, BuildEditContextCascadingValue, isFixed: true);
    }

    private void BuildEditContextCascadingValue(RenderTreeBuilder builder)
    {
        builder.CreateCascadingComponent(_fixedEditContext, 2, ChildContent?.Invoke(_fixedEditContext), isFixed: true);

        //builder.OpenComponent<CascadingValue<EditContext>>(2);
        //builder.AddAttribute(3, "Value", _fixedEditContext);
        //builder.AddAttribute(4, "IsFixed", true);
        //builder.AddAttribute(5, nameof(ChildContent), ChildContent?.Invoke(_fixedEditContext));
        //builder.CloseComponent();
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
