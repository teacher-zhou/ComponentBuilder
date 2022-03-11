using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        TestContext.RenderComponent<TestForm>()
            .Should().HaveTag("form");
    }
}

class TestForm : BlazorFormBase<TestForm>
{

}
