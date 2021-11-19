using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions.BUnit;
using Xunit;

namespace ComponentBuilder.Test.Components
{
    [ElementTag("button")]
    public class Button : BlazorComponentBase
    {
    }

    public class ComponentTest : ComponentBuilderTestBase
    {
        TestContext context = new();
        public ComponentTest()
        {
            context.Services.AddComponentBuilder();
        }

        [Fact]
        public void TestButtonElement()
        {
            context.RenderComponent<Button>().Should().HaveTag("button")
                   ;
        }
    }
}
