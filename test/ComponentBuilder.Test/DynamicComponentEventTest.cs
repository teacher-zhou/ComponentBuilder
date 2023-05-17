using ComponentBuilder.Automation.Abstrations;
using ComponentBuilder.Automation.Definitions;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Automation.Test
{
    public class DynamicComponentEventTest : AutoTestBase
    {
        //[Fact(Skip ="Skip this test")]
        ////[Fact]
        //public async void Given_Component_Has_Active_After_Invoke_Active_Method_When_Create_Component_Active_Manually_Then_Active_Is_True()
        //{
        //    var component = GetComponent<ActiveComponent>();

        //    await component.Instance.Activate();

        //    component.MarkupMatches("<div class=\"active\"></div>");
        //}

        class ActiveComponent : BlazorComponentBase, IHasOnActive
        {
            [Parameter] public EventCallback<bool> OnActive { get; set; }
            [Parameter][CssClass("active")] public bool Active { get; set; }
        }
    }
}
