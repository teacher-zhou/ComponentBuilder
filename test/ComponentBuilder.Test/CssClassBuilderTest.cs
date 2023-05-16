namespace ComponentBuilder.Automation.Test;

public class CssClassBuilderTest : AutoTestBase
{
    private readonly ICssClassBuilder _builder;
    public CssClassBuilderTest()
    {
        _builder = GetService<ICssClassBuilder>();
    }

    [Fact]
    public void Given_Invoke_Append_When_Value_Is_Null_Then_No_Css_Class_Return()
    {
        Assert.Empty(_builder.Append(null).ToString());
    }

    [Fact]
    public void Given_Invoke_Append_When_Input_Value_Twice_Then_After_Build_Get_The_Result_Separated_For_Each_Item()
    {
        _builder.Append("first").Append("second").ToString()
            .Should().Be("first second");
    }

    [Fact]
    public void Given_Invoke_Insert_When_Append_Value_Twice_And_Insert_To_First_Then_InsertValue_Should_Be_First_Of_Append()
    {
        _builder.Append("first").Append("second").Insert(0, "insert").ToString().Should().Be("insert first second");
    }

    [Fact]
    public void Append_When_Duplicate_Value_Then_Only_One_Should_Be_Appear()
    {
        _builder.Append("first").Append("second").Append("first").Append("second")
            .ToString().Should().Be("first second");
    }
}
