namespace ComponentBuilder.Test.Components
{

    public class ButtonTest
    {
        TestContext context = new();
        public ButtonTest()
        {
            context.Services.AddComponentBuilder();
        }

        [Fact]
        public void TestCreateButtonComponent()
        {
            context.RenderComponent<Button>().Should().HaveTag("button")
                   ;
        }

        [Fact]
        public void TestButtonParameter()
        {
            context.RenderComponent<Button>(p => p.Add(b => b.Block, true))
                .Should().HaveClass("block")
                ;
        }

        [Fact]
        public void TestButtonByBuildCssCrtlass()
        {
            context.RenderComponent<Button>()
                .Should().NotHaveClass("active")
                ;

            context.RenderComponent<Button>(m => m.Add(p => p.Active, true))
                .Should().HaveClass("active");
        }

        [Fact]
        public void TestButtonHasChildContent()
        {
            context.RenderComponent<Button>(p => p.Add(m => m.ChildContent, "button"))
                .Should().HaveChildMarkup("button");
        }
    }
}
