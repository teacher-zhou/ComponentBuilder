using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using ComponentBuilder.Test.Components;

namespace ComponentBuilder.Test;
public class RenderComponentAttributeTest : TestBase
{
    [Fact]
    public void Given_Use_RenderComponent_Then_Create_A_Button_Component()
    {
        TestContext.RenderComponent<RenderTestComponent>()
            .Should().HaveTag("button");
    }

    [Fact]
    public void Given_Use_RenderComponent_When_Set_Active_Then_Create_Button_Component_With_Parameter_CssClass()
    {
        TestContext.RenderComponent<RenderTestComponent>(p => p.Add(m => m.Toogle, true).Add(m => m.Margin, 5)).Should().HaveTag("button")
            .And.HaveClass("my-toggle").And.HaveClass("m-5");
    }

    [RenderComponent(typeof(Button))]
    internal class RenderTestComponent : BlazorComponentBase
    {
        [Parameter][CssClass("my-toggle")] public bool Toogle { get; set; }

        [Parameter][CssClass("m-")] public int Margin { get; set; }
    }
}
