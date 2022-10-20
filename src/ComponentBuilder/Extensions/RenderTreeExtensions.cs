using ComponentBuilder.Rendering;

namespace ComponentBuilder;
public static class RenderTreeExtensions
{
    public static BlazorRenderTree Begin(this RenderTreeBuilder builder, string elementName, int sequence = 0)
    {
        var render = new BlazorRenderTree(builder);
        return render.Begin(elementName, sequence);
    }
    public static BlazorRenderTree Begin(this RenderTreeBuilder builder, Type componentType, int sequence = 0)
    {
        var render = new BlazorRenderTree(builder);
        return render.Open(componentType, sequence);
    }

    public static BlazorRenderTree Begin<TComponent>(this RenderTreeBuilder builder, int sequence = 0) where TComponent : IComponent
    {
        var render = new BlazorRenderTree(builder);
        return render.Begin<TComponent>(sequence);
    }
}
