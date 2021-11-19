using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilder.Abstrations
{
    public interface IComponentBuilder
    {
        void BuildComponent(object component, RenderTreeBuilder builder);
    }
}
