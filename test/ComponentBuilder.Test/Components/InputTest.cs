namespace ComponentBuilder.Test.Components;
public class InputTest:TestBase
{
    [Fact]
    public void Test_Value_Changed()
    {
        var initValue = "test";
        var newValue = default(string);
        var component= TestContext.RenderComponent<Input<string>>(b => b.Bind(p => p.Value, initValue, value => newValue = value))
            .Should().HaveTag("input");

        Assert.NotEqual(newValue, initValue);
    }
}
