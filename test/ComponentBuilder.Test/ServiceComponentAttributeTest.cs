using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test
{
    public class ServiceComponentAttributeTest : TestBase
    {
        [Fact]
        public void Given_Use_ServiceComponent_Then_Render_Implementation_Component()
        {
            TestContext.RenderComponent<ServiceCompoenet>().Should().HaveTag("ul");
        }

        [Fact]
        public void Given_Use_SerivceComponent_When_Set_Paraemter_Only_ImplementationComponent_Have_Then_Render_This_Parameter_As_Normal()
        {
            TestContext.RenderComponent<ServiceCompoenet>(ComponentParameter.CreateParameter("Active", true), ComponentParameter.CreateParameter(nameof(ServiceCompoenet.Toggle), true))
                .Should().HaveTag("ul").And.Subject.MarkupMatches(@"
<ul class=""btn-toggle active""></ul>
");
        }
    }

    [ServiceComponent]
    internal class ServiceCompoenet : BlazorComponentBase
    {
        [Parameter][CssClass("btn-toggle")] public bool Toggle { get; set; }
    }

    [HtmlTag("ul")]
    internal class ImplementationComponent : ServiceCompoenet
    {
        [Parameter][CssClass("active")] public bool Active { get; set; }
    }
}
