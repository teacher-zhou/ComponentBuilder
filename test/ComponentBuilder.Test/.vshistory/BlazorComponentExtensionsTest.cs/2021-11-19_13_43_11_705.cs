using FluentAssertions;
using Xunit;

namespace ComponentBuilder.Test
{
    public class BlazorComponentExtensionsTest
    {
        public BlazorComponentExtensionsTest()
        {

        }

        [Fact]
        public void TestEnumCssClassAttribute()
        {
            TestEnum.Class1.GetCssClass().Should().Be("class1");
            TestEnum.Class2.GetCssClass().Should().Be("myclass");


        }

        enum TestEnum
        {
            Class1,
            [CssClass("myclass")] Class2,
        }
    }
}
