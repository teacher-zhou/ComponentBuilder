using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.Components
{
    [ElementTag("button")]
    [CssClass("btn")]
    public class Button : BlazorComponentBase, IHasChildContent
    {
        [Parameter] [CssClass("btn-")] public Color? Color { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }
    }

    public enum Color
    {
        Primary,
        Secondary,
        Danger,
        Warning,
        Info,
        Dark,
        Light,
        Success
    }
}
