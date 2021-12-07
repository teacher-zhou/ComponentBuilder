using ComponentBuilder.Abstrations;

namespace ComponentBuilder.Test
{
    public class ElementAttributeResolverTest : TestBase
    {
        private readonly ElementAttributeAttributeResolver _attributeResolver;
        public ElementAttributeResolverTest()
        {
            _attributeResolver = GetService<ElementAttributeAttributeResolver>();
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
                Title = "abc",
                Href = "www.bing.com"
            });

            result.Should().ContainKey("title").And.ContainValue("abc");
            result.Should().ContainKey("href").And.ContainValue("www.bing.com");
        }
    }

    [ElementTag("a")]
    class ElementPropertyComponent : BlazorComponentBase
    {
        [ElementAttribute("title")] public string Title { get; set; }
        [ElementAttribute] public string Href { get; set; }
    }
}
