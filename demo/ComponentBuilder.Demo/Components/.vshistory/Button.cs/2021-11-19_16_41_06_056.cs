using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.Components
{
    [ElementTag("button")]
    [CssClass("btn")]
    public class Button : BlazorComponentBase
    {
        [Parameter] [CssClass("btn-")] public Color? Color { get; set; }
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
