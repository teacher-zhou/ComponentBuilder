using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilder.Test
{
    public class ServiceComponentAttributeTest : TestBase
    {
        [Fact]
        public void Given_Use_ServiceComponent_Then_Render_Implementation_Component()
        {
            TestContext.RenderComponent<ServiceComponent>().Should().HaveTag("ul");
        }

        [Fact]
        public void Given_Use_SerivceComponent_When_Set_Paraemter_Only_ImplementationComponent_Have_Then_Render_This_Parameter_As_Normal()
        {
            TestContext.RenderComponent<ServiceComponent>(ComponentParameter.CreateParameter("Toggle", true), ComponentParameter.CreateParameter("Active", true), ComponentParameter.CreateParameter(nameof(ServiceComponent.ChildContent), new RenderFragment(builder => builder.AddContent(0, "abcd"))))
                .Should().HaveTag("ul").And.Subject.MarkupMatches(@"
<ul class=""btn-toggle active""><span>abcd</span></ul>
");
        }
    }

    [ServiceComponent]
    internal class ServiceComponent : BlazorComponentBase, IHasChildContent
    {
        [Parameter][CssClass("btn-toggle")] public bool Toggle { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    [HtmlTag("ul")]
    internal class ImplementationComponent : ServiceComponent, IHasChildContent
    {
        [Parameter][CssClass("active")] public bool Active { get; set; }

        protected override void BuildCssClass(ICssClassBuilder builder)
        {
            builder.Append("active", Active);
        }

        protected override void AddContent(RenderTreeBuilder builder, int sequence)
        {
            builder.CreateElement(sequence, "span", ChildContent);
        }
    }
}
