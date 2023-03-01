using ComponentBuilder.Abstrations;
using ComponentBuilder.Definitions.Components;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test.Components
{
    [HtmlRole("alert")]
    public class Anchor : BlazorComponentBase,IAnchorComponent
    {
        [Parameter]public string? Href { get; set; }
        [Parameter]public AnchorTarget Target { get; set; }
    }
}
