namespace MyRazorLibrary.Test;

public class DivTest : TestBase<Div>
{
    [Fact]
    public void Test_Render_Div()
    {
        GetComponent().Should().HaveTag("div");
    }

    [Fact]
    public void Test_Render_With_Content()
    {
        GetComponent(m => m.AddChildContent("hello")).Should().HaveTag("div").And.HaveChildMarkup("hello");
    }
}