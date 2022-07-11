

namespace ComponentBuilder.Test
{
    public class RenderTreeBuilderExtensionTest : TestBase
    {
        [Fact]
        public void Test_CreateElement_For_String()
        {
            TestContext.RenderComponent<RenderTreeBuilderComponent>(p => p.AddChildContent(builder =>
            {
                builder.CreateElement(0, "div", "test");
            }))
                .MarkupMatches("<div><div>test</div></div>")
                ;
        }
        [Fact]
        public void Test_CreateElement_For_Fragment()
        {
            TestContext.RenderComponent<RenderTreeBuilderComponent>(p => p.AddChildContent(builder =>
            {
                builder.CreateElement(0, "div", b => b.AddContent(0, "test"));
            }))
                .MarkupMatches("<div><div>test</div></div>")
                ;
        }

        [Fact]
        public void Test_CreateComponent_For_String()
        {
            TestContext.RenderComponent<RenderTreeBuilderComponent>(p => p.AddChildContent(builder =>
            {
                builder.CreateComponent<RenderTreeBuilderComponent>(0, "test");
            }))
                .MarkupMatches("<div><div>test</div></div>")
                ;
        }
        [Fact]
        public void Test_CreateComponent_For_Fragment()
        {
            TestContext.RenderComponent<RenderTreeBuilderComponent>(p => p.AddChildContent(builder =>
            {
                builder.CreateComponent(typeof(RenderTreeBuilderComponent), 0, b => b.AddContent(0, "test"));
            }))
                .MarkupMatches("<div><div>test</div></div>")
                ;
        }
    }

    internal class RenderTreeBuilderComponent : BlazorChildContentComponentBase
    {

    }
}
