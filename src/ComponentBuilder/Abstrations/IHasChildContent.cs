using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Abstrations
{
    public interface IHasChildContent
    {
        RenderFragment ChildContent { get; set; }
    }
}
