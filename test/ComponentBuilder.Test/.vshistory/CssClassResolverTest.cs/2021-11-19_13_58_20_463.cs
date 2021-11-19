using ComponentBuilder.Abstrations;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ComponentBuilder.Test
{
    public class CssClassResolverTest : ComponentBuilderTestBase
    {
        private readonly ICssClassResolver _resolver;
        public CssClassResolverTest()
        {
            _resolver = GetService<ICssClassResolver>();
        }

        [Fact]
        public void When_Component_For_Parameter_Mark_CssClass_When_Input_A_String_Then_Combile_Two_Of_Them()
        {
            var testComponent = new TestComponent();
            testComponent.Name = "abc";

            var result = _resolver.Resolve(testComponent.GetType());
            result.Should().Be("cssabc");
        }
    }

    class TestComponent : BlazorComponentBase
    {
        [CssClass("css")] public string Name { get; set; }
    }
}
