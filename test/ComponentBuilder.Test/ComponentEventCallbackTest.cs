using ComponentBuilder.Abstrations;
using ComponentBuilder.Definitions.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test
{
    public class ComponentEventCallbackTest : TestBase
    {
        [Fact]
        public void Invoke_Onclick_Event_When_OnClick_HasCallback_Then_OnClick_Invoke()
        {
            var clicked = false;
            TestContext.RenderComponent<ComponentEventCallback>(m => m.Add(p => p.OnClick, HtmlHelper.Event.Create<MouseEventArgs>(this,() => clicked=true)))
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
}
