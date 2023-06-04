namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// 提供渲染树其他特性的构造器。
/// </summary>
public interface IFluentFrameBuilder: IFluentContentBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// 将指定的键值分配给当前元素或组件。
    /// </summary>
    /// <param name="value">该键的值。</param>
    /// <returns><see cref="IFluentAttributeBuilder"/> 实例。</returns>
    IFluentAttributeBuilder Key(object? value);

    /// <summary>
    /// 捕获元素或组件的引用。
    /// </summary>
    /// <param name="captureReferenceAction">在呈现组件后捕获元素或组件引用的操作。</param>
    /// <returns><see cref="IFluentAttributeBuilder"/> 实例。</returns>
    IFluentAttributeBuilder Ref(Action<object?> captureReferenceAction);
}
