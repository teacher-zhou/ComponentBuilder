using ComponentBuilder.Parameters;

using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test;
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
        })
            .MarkupMatches("<div><div></div></div>");
    }

    [Fact]
    public void Given_Parent_Active_Child_Component_When_Parent_Index_Is_One_Then_Child_Component_At_First_Index_Is_Actived()
    {
        var tab = TestContext.RenderComponent<TabComponent>(builder =>
         {
             builder.AddChildContent(tab =>
             {
                 tab.CreateComponent<TabItemComponent>(1);
                 tab.CreateComponent<TabItemComponent>(2);
             });
         })
            ;

        tab.MarkupMatches(@"
<tab>
    <tabitem class=""active""></tabitem>
    <tabitem></tabitem>
</tab>
");


    }
}

class ParentComponent : BlazorParentComponentBase<ParentComponent, ChildComponent>, IHasChildContent
{
    [Parameter] public RenderFragment ChildContent { get; set; }
}

class ChildComponent : BlazorChildComponentBase<ParentComponent, ChildComponent>, IHasChildContent
{
    [Parameter] public RenderFragment ChildContent { get; set; }
}
[HtmlTag("tab")]
class TabComponent : BlazorParentComponentBase<TabComponent, TabItemComponent>, IHasChildContent
{
    [Parameter] public RenderFragment ChildContent { get; set; }
}
[HtmlTag("tabitem")]
class TabItemComponent : BlazorChildComponentBase<TabComponent, TabItemComponent>, IHasChildContent, IHasOnActive
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public bool Active { get; set; }
    public EventCallback<bool> OnActive { get; set; }
}

