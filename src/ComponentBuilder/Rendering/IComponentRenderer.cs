namespace ComponentBuilder.Rendering;

/// <summary>
/// Provides component rendering based on <see cref="RenderTreeBuilder"/> render tree.
/// </summary>
public interface IComponentRenderer
{
    /// <summary>
    /// Use <see cref="RenderTreeBuilder"/> to render the component.
    /// </summary>
    /// <param name="component">The component to render.</param>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> instance.</param>
    /// <returns>Return <c>true</c> if you need to continue rendering the next component from the pipeline, otherwise return <c>false</c>.</returns>
    bool Render(IBlazorComponent component, RenderTreeBuilder builder);
}
