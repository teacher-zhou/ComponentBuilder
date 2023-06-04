namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// 提供元素内容的构造器。
/// </summary>
public interface IFluentContentBuilder :IFluentOpenBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// 向这个元素或组件添加片段内容。多个内容将被合并用于多个调用。
    /// </summary>
    /// <param name="fragment">要插入内部元素的内容片段。</param>
    /// <returns>包含片段内容的 <see cref="IFluentOpenBuilder"/> 实例。</returns>
    IFluentContentBuilder Content(RenderFragment? fragment);
}

/// <summary>
/// 提供为指定组件类型添加子内容的构造器。
/// </summary>
/// <typeparam name="TComponent"></typeparam>
public interface IFluentContentBuilder<TComponent> : IFluentContentBuilder,IFluentOpenComponentBuilder<TComponent> 
    where TComponent : IComponent
{
    /// <summary>
    /// 为组件的 <c>ChildContent</c> 参数添加任意代码片段。
    /// </summary>
    /// <param name="fragment">要添加的任意代码片段。</param>
    /// <returns>包含片段内容的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    new IFluentAttributeBuilder<TComponent> Content(RenderFragment? fragment);
}