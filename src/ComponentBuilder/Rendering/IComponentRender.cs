namespace ComponentBuilder.Rendering;

/// <summary>
/// 提供基于 <see cref="RenderTreeBuilder"/> 渲染树的组件渲染的功能。
/// </summary>
public interface IComponentRender
{
    /// <summary>
    /// 使用 <see cref="RenderTreeBuilder"/> 渲染组件。
    /// </summary>
    /// <param name="component">要渲染的组件。</param>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <returns>如果需要继续从管道中渲染下一个组件，则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    bool Render(IBlazorComponent component, RenderTreeBuilder builder);
}
