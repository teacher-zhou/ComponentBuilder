using Bunit;
using ComponentBuilder.Abstrations;
using FluentAssertions.BUnit;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace ComponentBuilder.Test.Components
{
    [ElementTag("button")]
    public class Button : BlazorComponentBase
    {

        [Parameter] [CssClass("block")] public bool Block { get; set; }

        [Parameter] public bool Active { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            if (Active)
            {
                builder.Append("active");
            }
        }
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

        [Fact]
        public void TestButtonByBuildCssCrtlass()
        {
            context.RenderComponent<Button>()
                .Should().NotHaveClass("active")
                ;

            context.RenderComponent<Button>(m => m.Add(p => p.Active, true))
                .Should().HaveClass("active");
        }
    }
}
