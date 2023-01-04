using ComponentBuilder.Abstrations;
using ComponentBuilder.Parameters;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ComponentBuilder.Demo.ServerSide.Components
{
    [HtmlTag("form")]
    [ParentComponent]
    public class TestForm : BlazorComponentBase, IHasForm
    {
        [Parameter] public object? Model { get; set; }
        [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
        [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }
        [Parameter] public EditContext? EditContext { get; set; }
        [Parameter] public RenderFragment<EditContext>? ChildContent { get; set; }
    }
}
