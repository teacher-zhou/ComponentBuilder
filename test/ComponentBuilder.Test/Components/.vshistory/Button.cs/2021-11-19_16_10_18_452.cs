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
            context.RenderComponent<Button>(p => p.Add(b => b.Block, true))
                .Should().HaveClass("block")
                ;
        }
    }
}
