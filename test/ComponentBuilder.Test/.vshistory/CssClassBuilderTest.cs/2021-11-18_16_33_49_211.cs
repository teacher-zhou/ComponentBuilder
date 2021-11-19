using ComponentBuilder.Abstrations;
using FluentAssertions;
using System;
using Xunit;

namespace ComponentBuilder.Test
{
    public class CssClassBuilderTest
    {
        private readonly ICssClassBuilder _builder;
        public CssClassBuilderTest()
        {
            _builder = new DefaultCssClassBuilder();
        }
        [Fact]
        public void TestAdd()
        {
            var result = _builder.Append("class1").Append("class2").Build();
            result.Should().Be("class1 class2");
        }
    }
}
