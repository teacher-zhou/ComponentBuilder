namespace ComponentBuilder.Test
{
    public class TreeRenderTest:TestBase
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
                            .End();

                    content.Begin("div")
                            .Class((false, "active"), "disabled")
                            .Content(button =>
                            {
                                button.Begin("button").Class("btn").Content("submit").End();
                            })
                            .End();
                }).End();
                    
            }).MarkupMatches(@"
<div class=""ab"">
    <span>abc</span>
    <div class=""disabled"">
        <button class=""btn"">submit</button>
    </div>
</div>
");
        }
    }
}
