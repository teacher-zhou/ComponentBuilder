using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [HtmlTag("table")]
    [ParentComponent]
    public class Table : BlazorComponentBase, IHasChildContent
    {
        [Parameter]public RenderFragment? ChildContent { get; set; }
    }

    [HtmlTag("tr")]
    [ParentComponent]
    [ChildComponent(typeof(Table))]
    public class TableRow : BlazorComponentBase, IHasChildContent
    {
        [CascadingParameter]public Table? CascadingTable { get; set; }
        [Parameter]public RenderFragment? ChildContent { get; set; }
    }

    [HtmlTag("td")]
    [ChildComponent(typeof(TableRow))]
    public class TableCell:BlazorComponentBase, IHasChildContent
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
