namespace ComponentBuilder.Test
{
    public class BlazorRenderTreeTest : TestBase
    {
        [Fact]
        public void Test_Gramma()
        {
            TestContext.Render(m =>
            {
                m.Begin("div").Class("ab").Content(content =>
                {
                    content.Begin("span")
                            .Content("abc")
                            .Close();

                    content.Begin("div")
                            .Class((false, "active"), "disabled")
                            .Content(button =>
                            {
                                button.Begin("button").Class("btn").Content("submit").Close();
                            })
                            .Close();
                }).Close();

            }).MarkupMatches(@"
<div class=""ab"">
    <span>abc</span>
    <div class=""disabled"">
        <button class=""btn"">submit</button>
    </div>
</div>
");

            TestContext.Render(builder =>
            {
                builder.Begin("div").Class("me").Class("me").Content("hello").Close();
            }).MarkupMatches(@"
<div class=""me"">hello</div>
");
        }


    }
}
