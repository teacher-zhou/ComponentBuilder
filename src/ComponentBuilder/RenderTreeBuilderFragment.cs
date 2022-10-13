namespace ComponentBuilder;

/// <summary>
/// 表示构造 <see cref="RenderTreeBuilder"/> 渲染树的片段。
/// </summary>
public sealed class RenderTreeBuilderFragment
{
    private int _startSequence;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenderTreeBuilderFragment"/> class.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="startSequence">表示指令在源代码中的位置的整数。</param>
    internal RenderTreeBuilderFragment(RenderTreeBuilder builder,in int startSequence)
    {
        this.Builder = builder;
        this._startSequence = startSequence;
    }

    /// <summary>
    /// 获取 <see cref="RenderTreeBuilder"/> 实例。
    /// </summary>
    public RenderTreeBuilder Builder { get; }
    /// <summary>
    /// 获取操作过后的最后一个源代码位置的序列。
    /// </summary>
    public int LastSequence => _startSequence;

    /// <summary>
    /// 将指定的键值赋给当前元素或组件。
    /// </summary>
    /// <param name="value">键的值。</param>
    public void SetKey(object? value) => Builder.SetKey(value);

    /// <summary>
    /// 附加一个表示捕获父元素引用的指令的帧。
    /// </summary>
    /// <param name="elementReferenceCaptureAction">每当引用值更改时要调用的操作。</param>
    public void AddElementReferenceCapture(Action<ElementReference> elementReferenceCaptureAction) => Builder.AddElementReferenceCapture(++_startSequence, elementReferenceCaptureAction);

    /// <summary>
    /// 附加一个表示捕获父组件引用的指令的帧。
    /// </summary>
    /// <param name="componentReferenceCaptureAction">每当引用值更改时要调用的操作。</param>
    public void AddComponentReferenceCapture(Action<object> componentReferenceCaptureAction)
        =>Builder.AddComponentReferenceCapture(++_startSequence, componentReferenceCaptureAction);
    /// <summary>
    /// 指示前面的属性表示一个事件处理程序，该事件处理程序的执行使用名称更新属性。
    /// <para>
    /// 呈现系统使用此信息来确定在接收到对事件处理程序的调用时是否接受其他属性的值更新。
    /// </para>
    /// </summary>
    /// <param name="updatesAttributeName">在执行事件处理程序时可更新其值的另一个属性的名称。</param>
    public void SetUpdatesAttributeName(string updatesAttributeName)
        =>Builder.SetUpdatesAttributeName(updatesAttributeName);
}
