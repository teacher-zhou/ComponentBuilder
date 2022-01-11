using ComponentBuilder.Abstrations;

namespace ComponentBuilder.Test
{
    public class HtmlAttributeResolverTest : TestBase
    {
        private readonly IHtmlAttributesResolver _attributeResolver;
        public HtmlAttributeResolverTest()
        {
            _attributeResolver = GetService<IHtmlAttributesResolver>();
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

        [Fact]
        public void Given_Invoke_Resolve_When_Component_Drop_Is_Bool_Then_DataToggle_Has_Value_Of_Drop()
        {
            _attributeResolver.Resolve(new ElementPropertyComponent
            {
                Drop = true
            }).Should().ContainKey("data-toggle").And.ContainValue("drop");
        }

        [Fact]
        public void When_Enum_Has_HtmlAttribute_Then_Parameter_Is_Enum_To_Use_HtmlAttribute()
        {
            _attributeResolver.Resolve(new ElementPropertyComponent
            {
                Target = ElementPropertyComponent.LinkTarget.Blank
            }).Should().ContainKey("target").And.ContainValue("_blank");
        }
    }

    [HtmlTag("a")]
    class ElementPropertyComponent : BlazorComponentBase
    {
        [HtmlAttribute("title")] public string Title { get; set; }
        [HtmlAttribute] public string Href { get; set; }

        [HtmlData("toggle")]public bool Drop { get; set; }

        [HtmlAttribute] public LinkTarget? Target { get; set; }

        public enum LinkTarget
        {
            [HtmlAttribute("_blank")]Blank,
            [HtmlAttribute("_self")]Self
        }
    }
}
