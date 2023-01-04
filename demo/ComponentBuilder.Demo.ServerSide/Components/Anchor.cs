using ComponentBuilder.Abstrations;
using ComponentBuilder.Parameters;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [HtmlTag("a")]
    public class Anchor : BlazorComponentBase, IHasNavLink, IHasChildContent
    {
        [Parameter][HtmlAttribute("href")] public string Link { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Parameter] public NavLinkMatch Match { get; set; } = NavLinkMatch.All;
        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append("text-danger", IsNavLinkActived);
        }
    }
}
