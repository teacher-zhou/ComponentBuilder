using ComponentBuilder.Builder;
using ComponentBuilder.Definitions.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [HtmlTag("a")]
    public class Anchor : BlazorComponentBase, IHasNavLink, IHasChildContent, IHasAdditionalClass
    {
        [Parameter][HtmlAttribute("href")] public string Link { get; set; }
        [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.All;
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public string? AdditionalClass { get; set; }
        public bool IsActive { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            if ( IsActive )
            {
                builder.Append("active");
            }
        }

        protected override void BuildAttributes(IDictionary<string, object> attributes)
        {
        }
    }
}
