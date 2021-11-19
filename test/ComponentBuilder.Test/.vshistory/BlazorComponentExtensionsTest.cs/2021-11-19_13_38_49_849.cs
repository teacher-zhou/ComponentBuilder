using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
