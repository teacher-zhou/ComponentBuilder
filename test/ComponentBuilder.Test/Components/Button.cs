using Bunit;
using ComponentBuilder.Abstrations;
using ComponentBuilder.Attributes;
using FluentAssertions.BUnit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace ComponentBuilder.Test.Components
{
    [ElementTag("button")]
    public class Button : BlazorComponentBase, IHasChildContent
    {

        [Parameter] [CssClass("block")] public bool Block { get; set; }

        [Parameter] public bool Active { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            if (Active)
            {
                builder.Append("active");
            }
        }
    }

}
