

using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Test;
public class BlazorComponentBaseTest : TestBase
{
    [Fact]
    public void When_Component_Has_RenderComponentAttribute_Then_Create_Component_With_This_Attribute()
    {
        TestContext.RenderComponent<TestComponentWithRenderComponent>()
            .MarkupMatches("<a></a>");
    }
}

[RenderComponent(typeof(NavLink))]
class TestComponentWithRenderComponent : BlazorComponentBase
{

}