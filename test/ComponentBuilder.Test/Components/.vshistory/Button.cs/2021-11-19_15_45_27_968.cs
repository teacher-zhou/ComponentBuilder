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
    }
}
