using ComponentBuilder.Forms;

namespace ComponentBuilder.Test;
public class FormComponentTest : TestBase
{
    public FormComponentTest()
    {

    }

    [Fact]
    public void Given_A_Form_Then_Has_Form_Element_Tag()
    {
        TestContext.RenderComponent<TestForm>(p => p.Add(m => m.Model, this))
            .Should().HaveTag("form");
    }
}

class TestForm : BlazorFormBase<TestForm>
{

}
