

namespace ComponentBuilder.Test
{
    public class RenderTreeBuilderExtensionTest : TestBase
    {
        [Fact]
        public void Test_CreateElement()
        {
            TestContext.Render(builder => builder.CreateElement(0, "div", "abc"))
                .MarkupMatches("<div>abc</div>");

            TestContext.Render(builder => builder.CreateElement(0, "div", "abc", attributes =>
            {
                attributes["class"] = "myclass";
            })).MarkupMatches("<div class=\"myclass\">abc</div>");

            TestContext.Render(builder => builder.CreateElement(0, "div", "abc", new { @class = "myclass" })).MarkupMatches("<div class=\"myclass\">abc</div>");


            TestContext.Render(builder => builder.CreateDiv(0, "abc", new { @class = "myclass" })).MarkupMatches("<div class=\"myclass\">abc</div>");

            TestContext.Render(builder => builder.CreateSpan(0, "abc", new { @class = "myclass" })).MarkupMatches("<span class=\"myclass\">abc</span>");

            TestContext.Render(b => b.CreateParagraph(0, "aaa")).MarkupMatches("<p>aaa</p>");

            TestContext.Render(b => b.CreateHr(0)).MarkupMatches("<hr/>");

            TestContext.Render(b => b.CreateBr(0)).MarkupMatches("<br/><br/>");
        }
    }
}
