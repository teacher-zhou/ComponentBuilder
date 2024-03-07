namespace ComponentBuilder.Test;

public class StylePropertyTest
{
    [Fact]
    public void Test_Style()
    {
        var style = new StyleProperty
        {
            ["margin"] = "5px",
            ["margin-top"] = 20,
            ["border"] = false
        };

        Assert.Equal("margin:5px;margin-top:20", style.ToString());
    }

    [Fact]
    public void Test_String_To_String()
    {
        StyleProperty style = "margin:5px; margin-top: 20px";

        Assert.Equal("5px", style["margin"]);
        Assert.Equal("20px", style["margin-top"]);
    }
}
