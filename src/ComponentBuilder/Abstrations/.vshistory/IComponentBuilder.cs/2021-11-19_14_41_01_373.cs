using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilder.Abstrations
{
    public interface IComponentBuilder
    {
        void BuildComponent(RenderTreeBuilder builder);
    }
}
