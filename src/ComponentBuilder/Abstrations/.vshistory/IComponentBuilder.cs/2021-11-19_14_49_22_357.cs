using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilder.Abstrations
{
    public interface IComponentBuilder
    {
        void BuildComponent<TComponent>(TComponent component, RenderTreeBuilder builder) where TComponent : BlazorComponentBase;
    }
}
