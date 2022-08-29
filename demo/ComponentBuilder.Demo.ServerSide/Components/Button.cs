using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [CssClass("btn")]
    public class Button : BlazorComponentBase, IHasChildContent
    {
        protected override string TagName => "button";
        [Parameter][CssClass("btn-")] public Color? Color { get; set; }

        [Parameter][CssClass("active")] public bool Active { get; set; }

        [Parameter] public bool HasToggle { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append("active toggle", HasToggle);
        }

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
