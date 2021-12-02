using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentBuilder.Abstrations;
using ComponentBuilder.Attributes;

namespace ComponentBuilder.Test
{
    public class ElementPropertyResolverTest : ComponentBuilderTestBase
    {
        private readonly ElementPropertyAttributeResolver _attributeResolver;
        public ElementPropertyResolverTest()
        {
            _attributeResolver = GetService<ElementPropertyAttributeResolver>();
        }

        [Fact]
        public void Given_Invoke_Resolve_When_Arg_Is_Null_Then_Throw_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => _attributeResolver.Resolve(null));
        }

        [Fact]
        public void Given_Invoke_Resolve_When_Component_Input_Title_Then_Title_Has_Value()
        {
            var result = _attributeResolver.Resolve(new ElementPropertyComponent
            {
                Title = "abc"
            });

            result.Should().ContainKey("title");
            result.Should().ContainValue("abc");
        }
    }

    [ElementTag("a")]
    class ElementPropertyComponent : BlazorComponentBase
    {
        [ElementProperty("title")] public string Title { get; set; }
    }
}
