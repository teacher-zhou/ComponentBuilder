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

        [Fact]
        public void Given_Create_Element_When_Multiple_Content_Then_Get_Last_Content()
        {
            TestContext.Render(builder =>
            {
                builder.Open("div")
                        .Class("my-class")
                            .Content("content1")
                            .Content("content2")
                            .Content("content3")
                       .Close();
            }).MarkupMatches("<div class=\"my-class\">content1content2content3</div>");
        }

        [Fact]
        public void Given_Create_Element_When_Multiple_Class_Then_Exception_Throw()
        {
            Assert.Throws<InvalidOperationException>(() => TestContext.Render(builder =>
             {
                 builder.Open("div").Class("class1").Class("class2");
             }));
        }
    }
}
