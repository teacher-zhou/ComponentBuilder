namespace ComponentBuilder.Test
{
    public class ParentChildComponentTest : TestBase
    {
        public ParentChildComponentTest()
        {

        }

        [Fact]
        public void When_Child_Component_Not_In_Parent_Component_Then_Throw_InvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => TestContext.RenderComponent<ChildComponent>());

        }

        [Fact]
        public void When_Child_Component_In_Parent_Component_Then_Render()
        {
            TestContext.RenderComponent<ParentComponent>(builder =>
            {
                builder.AddChildContent(component => component.CreateComponent<ChildComponent>(1));
            });
        }
    }

    class ParentComponent : BlazorParentComponentBase<ParentComponent, ChildComponent>
    {

    }

    class ChildComponent : BlazorChildComponentBase<ParentComponent, ChildComponent>
    {

    }


}
