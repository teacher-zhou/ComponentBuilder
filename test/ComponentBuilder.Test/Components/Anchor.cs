using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    [ElementTag("a")]
    [ElementRole("alert")]
    public class Anchor : BlazorComponentBase
    {
        [ElementProperty("title")] [Parameter] public string Title { get; set; }

        [ElementProperty("href")] [Parameter] public string Link { get; set; }
    }
}
