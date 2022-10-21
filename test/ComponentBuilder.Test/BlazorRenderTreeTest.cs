namespace ComponentBuilder.Test
{
    public class BlazorRenderTreeTest : TestBase
    {
        [Fact]
        public void Test_Create_Empty_Element()
        {
            var matchMarkup = "<div></div>";
            TestContext.Render(builder =>
            {
                builder.Open("div").Close();
            }).MarkupMatches(matchMarkup);

            TestContext.Render(builder =>
            {
                using var div= builder.Open("div");
            }).MarkupMatches(matchMarkup);

            TestContext.Render(builder =>
            {
                using ( var div = builder.Open("div") )
                {
                }
            }).MarkupMatches(matchMarkup);
        }

        [Fact]
        public void Test_Create_Element_With_Specified_Class()
        {
            TestContext.Render(builder =>
            {
                builder.Open("div").Class("my-class", "my-class1").Content("hello").Close();
            }).MarkupMatches("<div class=\"my-class my-class1\">hello</div>");
        }

        [Fact]
        public void Test_Create_Element_For_ChildContent()
        {
            TestContext.Render(builder =>
            {
                builder.Open("div")
                        .Class("class1")
                        .Content(span => span.Open("span")
                                                .Content("hello")
                                            .Close())
                .Close();
            }).MarkupMatches("<div class=\"class1\"><span>hello</span></div>");
        }
    }
}
