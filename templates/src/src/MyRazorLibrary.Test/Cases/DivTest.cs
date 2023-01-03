namespace MyRazorLibrary.Test.Cases;

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

    [Fact]
    public void Test_Has_AdditionalClass()
    {
        GetComponent(m => m.Add(p => p.AdditionalClass, "test-class")).Should().HaveClass("test-class");
    }
    [Fact]
    public void Test_Has_AdditionalStyle()
    {
        GetComponent(m => m.Add(p => p.AdditionalStyle, "width:100px;font-size:2rem")).Should().HaveAttribute("style,","width:100px;font-size:2rem");
    }
}