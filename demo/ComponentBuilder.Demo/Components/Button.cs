using ComponentBuilder.Abstrations;
using ComponentBuilder.Attributes;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.Components
{
    [ElementTag("button")]
    [CssClass("btn")]
    public class Button : BlazorComponentBase, IHasChildContent
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] [CssClass("btn-")] public Color? Color { get; set; }

        [Parameter] [CssClass("active")] public bool Active { get; set; }
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
