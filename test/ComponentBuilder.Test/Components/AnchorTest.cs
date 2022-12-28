using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test.Components
{
    public class AnchorTest
    {
        readonly TestContext context = new();
        public AnchorTest()
        {
            context.Services.AddComponentBuilder();
        }

        [Fact]
        public void Given_Anchor_Is_Alert_Role_When_Input_Title_Link_Then_Has_Title_Href_Role_Attribute_In_Element()
        {
            var anchor = context.RenderComponent<Anchor>(
                ComponentParameter.CreateParameter("Title", "tip"),
                ComponentParameter.CreateParameter("Link", "www.bing.com")
                );

            anchor.Should().HaveAttribute("title", "tip").And.HaveAttribute("href", "www.bing.com")
                .And.HaveAttribute("role", "alert")
                ;
        }
    }
}
