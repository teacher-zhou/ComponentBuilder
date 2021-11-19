using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions.BUnit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace ComponentBuilder.Test.Components
{
    [ElementTag("button")]
    public class Button : BlazorComponentBase
    {

        [Parameter] [CssClass("block")] public bool Block { get; set; }

    }

    public class ComponentTest
    {
        TestContext context = new();
        public ComponentTest()
        {
            context.Services.AddComponentBuilder();
        }

        [Fact]
        public void TestCreateButtonComponent()
        {
            context.RenderComponent<Button>().Should().HaveTag("button")
                   ;
        }

        [Fact]
        public void TestButtonParameter()
        {
            context.RenderComponent<Button>(ComponentParameter.CreateParameter(nameof(Button.Block), true))
                .Should().HaveClass("block");
        }
    }
}
