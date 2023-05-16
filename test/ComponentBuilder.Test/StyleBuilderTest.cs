namespace ComponentBuilder.Automation.Test;

public class StyleBuilderTest:AutoTestBase
{
    private readonly IStyleBuilder _builder;
    public StyleBuilderTest()
    {
        _builder=GetService<IStyleBuilder>();
    }

    [Fact]
    public void Given_Invoke_Append_When_Value_Is_Null_Then_No_Style_Return()
    {
        Assert.Empty(_builder.Append(null).ToString());
    }

    [Fact]
    public void Given_Invoke_Append_When_Input_Value_Twice_Then_After_Build_Get_The_Result_Separated_For_Each_Item()
    {
        _builder.Append("width:10px").Append("height:20px").ToString()
            .Should().Be("width:10px;height:20px");
    }
}
