using ComponentBuilder.Abstrations;
using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace ComponentBuilder.Test;
public class FormComponentTest : TestBase
{
    public FormComponentTest()
    {

    }

    [Fact]
    public void Given_A_Form_Then_Has_Form_Element_Tag()
    {
        TestContext.RenderComponent<TestForm>(p => p.Add(m => m.Model, this))
            .Should().HaveTag("form");
    }
}
[HtmlTag("form")]
class TestForm : BlazorComponentBase, IHasForm
{
    [Parameter]public object? Model { get; set; }
    [Parameter] public EventCallback<EditContext> OnSubmit { get; set; }
    [Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }
    [Parameter] public EventCallback<EditContext> OnInvalidSubmit { get; set; }
    [Parameter] public EditContext? EditContext { get; set; }
    [Parameter]
    public RenderFragment<EditContext>? ChildContent { get; set; }
    public EditContext? FixedEditContext { get; set; }
}
