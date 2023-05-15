using ComponentBuilder.Automation.Abstrations;
using ComponentBuilder.Automation.Definitions;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Automation.Test.Components
{
    [HtmlRole("alert")]
    public class Anchor : BlazorComponentBase,IAnchorComponent
    {
        [Parameter]public string? Href { get; set; }
        [Parameter]public AnchorTarget Target { get; set; }
    }
}
