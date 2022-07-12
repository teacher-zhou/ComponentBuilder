using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [HtmlTag("a")]
    public class Anchor:BlazorAnchorComponentBase
    {
        [Parameter][HtmlAttribute("href")]public string Link { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append("text-danger", IsActive);
        }
    }
}
