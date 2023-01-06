using ComponentBuilder.Abstrations;
using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    public class Button : BlazorComponentBase, IHasChildContent
    {
        protected override string? GetTagName()=>"button";
        [Parameter][CssClass("block")] public bool Block { get; set; }

        [Parameter] public bool Active { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            if (Active)
            {
                builder.Append("active");
            }
        }
    }

}
