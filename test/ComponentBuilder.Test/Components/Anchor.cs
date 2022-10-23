

using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    [HtmlRole("alert")]
    public class Anchor : BlazorAbstractComponentBase
    {
        protected override string TagName => "a";
        [HtmlAttribute("title")] [Parameter] public string Title { get; set; }

        [HtmlAttribute("href")] [Parameter] public string Link { get; set; }
    }
}
