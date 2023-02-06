using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    [HtmlRole("alert")]
    public class Anchor : BlazorComponentBase
    {
        public override string? GetTagName() => "a";

        [HtmlAttribute("title")] [Parameter] public string Title { get; set; }

        [HtmlAttribute("href")] [Parameter] public string Link { get; set; }
    }
}
