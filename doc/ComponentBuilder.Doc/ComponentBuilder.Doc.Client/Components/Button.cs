using ComponentBuilder.Definitions;

using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Doc.Client.Components
{
    [HtmlTag("button")]
    [CssClass("btn")]
    public class Button : BlazorComponentBase, IHasChildContent, IHasAdditionalStyle
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public StyleProperty? AdditionalStyle { get; set; }
    }
}
