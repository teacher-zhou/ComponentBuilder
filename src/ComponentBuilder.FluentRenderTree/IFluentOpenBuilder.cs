namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// 提供创建元素或组件开始标记的构造器。
/// </summary>
public interface IFluentOpenBuilder : IFluentOpenElementBuilder,IFluentOpenComponentBuilder
{
    /// <summary>
    /// 添加片段内容。
    /// </summary>
    /// <param name="fragment">要插入内部元素的内容片段。</param>
    /// <param name="sequence">表示源代码位置的序列。</param>
    /// <returns>包含片段内容的 <see cref="IFluentOpenBuilder"/> 实例。</returns>
    IFluentOpenBuilder Content(RenderFragment? fragment, int? sequence = default);
}
