using ComponentBuilder.Automation.Definitions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ComponentBuilder.Automation.Test;
public class ComponentEventCallbackTest : AutoTestBase
{
    [Fact]
    public void Invoke_Onclick_Event_When_OnClick_HasCallback_Then_OnClick_Invoke()
    {
        var clicked = false;
        TestContext.RenderComponent<ComponentEventCallback>(m => m.Add(p => p.OnClick, HtmlHelper.CreateCallback().Create<MouseEventArgs>(this,() => clicked=true)))
            .Find("div").Click()
            ;
        Assert.True(clicked);
    }
}

class ComponentEventCallback : BlazorComponentBase, IHasOnClick,IHasTest
{
    [Parameter][HtmlAttribute("onclick")]public EventCallback<MouseEventArgs> OnClick { get; set; }
    [HtmlAttribute("ondbclick")]public EventCallback OnTest { get; set; }
}

interface IHasTest
{
    [HtmlAttribute("ontest")]EventCallback OnTest { get; set; }
}
