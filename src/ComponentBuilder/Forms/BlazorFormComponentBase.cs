namespace ComponentBuilder.Forms;

/// <summary>
/// 提供表单组件的基类。与 <see cref="EditForm"/> 的逻辑相似。
/// </summary>
/// <typeparam name="TFormComponent">表单组件类型。</typeparam>
[HtmlTag("form")]
public abstract class BlazorFormComponentBase<TFormComponent> : BlazorComponentBase, IHasChildContent<EditContext>
    where TFormComponent : ComponentBase
{

    private EditContext _fixedEditContext;
    private readonly Func<Task> _handleSubmitDelegate;
    private bool _hasSetEditContextExplicitly;
    /// <summary>
    /// 初始化 <see cref="BlazorFormComponentBase{TForm}"/> 类的新实例。
    /// </summary>
    public BlazorFormComponentBase()
    {
        _handleSubmitDelegate = Submit;

    }
    /// <summary>
    ///  指定表单的顶级模型对象。将为这个模型构造一个编辑上下文。如果使用此参数，也不要提供 <see cref="EditContext"/> 的值。
    /// </summary>
    [Parameter] public object Model { get; set; }
    /// <summary>
    /// 显式提供编辑上下文。如果使用此参数，也不要提供 <see cref="Model"/>, 因为模型值将取自 <see cref="EditContext.Model"/> 属性。
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
    /// 提交表单时将调用的回调。
    /// 如果使用此参数，则由您负责手动触发任何验证，例如，通过调用 <see cref="EditContext.Validate"/> method.
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
    /// <summary>
    ///  当表单被提交时将被调用的回调函数，然后 <see cref="EditContext"/> 会被判断为有效。
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <summary>
    /// 当表单被提交时将被调用的回调函数，然后 <see cref="EditContext"/> 会被判断为无效。
    /// </summary>
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// 表单的内容。
    /// </summary>
    [Parameter] public RenderFragment<EditContext>? ChildContent { get; set; }

    /// <summary>
    /// 获取一个布尔值，表示表单是否处于提交状态。
    /// </summary>
    public bool IsSubmitting { get; private set; }

    protected override int RegionSequence => _fixedEditContext.GetHashCode();

    /// <summary>
    /// 当组件从呈现树中的父组件接收到参数，并且传入值已分配给属性时调用。
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// 当为{nameof(Form)}提供{nameof(OnSubmit)}参数时，不要同时提供{nameof(OnValidSubmit)}或{nameof(OnInvalidSubmit)}参数。
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


    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        this.CreateCascadingComponent<TFormComponent>(builder, sequence + 2, form =>
        {
            form.CreateCascadingComponent(_fixedEditContext, 0, content => content.AddContent(0, ChildContent?.Invoke(_fixedEditContext)), isFixed: true);
        }, isFixed: true);
    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onsubmit"] = _handleSubmitDelegate;
    }

    /// <summary>
    /// 执行表单提交。
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
