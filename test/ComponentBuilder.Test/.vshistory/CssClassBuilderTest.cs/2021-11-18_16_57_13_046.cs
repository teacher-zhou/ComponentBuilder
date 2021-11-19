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
        public void Given_Invoke_Append_When_Input_Null_Value_Then_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _builder.Append(null));
        }

    }
}
