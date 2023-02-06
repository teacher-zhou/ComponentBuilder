namespace ComponentBuilder.Rending;

/// <summary>
/// Defines that component rendered by <see cref="RenderTreeBuilder"/>.
/// </summary>
public interface IComponentRender
{
    /// <summary>
    /// Render component using <see cref="RenderTreeBuilder"/>.
    /// </summary>
    /// <param name="component">The component instance.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    /// <returns><c>True</c> to continue render next component from pipeline, otherwise, <c>false</c>.</returns>
    bool Render(IBlazorComponent component, RenderTreeBuilder builder);
}
