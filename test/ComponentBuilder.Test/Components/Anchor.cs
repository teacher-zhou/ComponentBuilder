using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    [ElementTag("a")]
    [ElementRole("alert")]
    public class Anchor : BlazorComponentBase
    {
        [ElementAttribute("title")] [Parameter] public string Title { get; set; }

        [ElementAttribute("href")] [Parameter] public string Link { get; set; }
    }
}
