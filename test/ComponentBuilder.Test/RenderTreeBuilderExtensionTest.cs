using System.Collections.Generic;

using ComponentBuilder.Parameters;

using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test
{
    public class RenderTreeBuilderExtensionTest : TestBase
    {
        [Fact]
        public void Test_CreateElement()
        {
            TestContext.Render(builder => builder.CreateElement(0, "div", "abc"))
                .MarkupMatches("<div>abc</div>");

            TestContext.Render(builder => builder.CreateElement(1, "div", childContent =>
            {
                childContent.CreateElement(0, "span", "test");
            })).MarkupMatches("<div><span>test</span></div");

            TestContext.Render(b => b.AddContent(0, content => content.AddContent(0, "")));
        }

        [Fact]
        public void Test_CreateElement_With_Anonymouse_Class()
        {
            TestContext.Render(builder => builder.CreateElement(0, "div", "abc", new { @class = "myclass" }))
                .MarkupMatches("<div class=\"myclass\">abc</div>");
        }

        [Fact]
        public void Test_CreateElement_With_Dictionary_Class()
        {
            TestContext.Render(builder =>
            builder.CreateElement(0, "div", "abc", new Dictionary<string, object>() { { "class", "myclass" } }))
                .MarkupMatches("<div class=\"myclass\">abc</div>");


            TestContext.Render(builder =>
            builder.CreateElement(0, "div", "abc", HtmlHelper.CreateHtmlAttributes(dic => dic["class"] = "myclass")))
                .MarkupMatches("<div class=\"myclass\">abc</div>");
        }

        [Fact]
        public void Test_CreateComponent()
        {
            TestContext.Render(builder => builder.CreateComponent<CreateComponent>(0))
                .MarkupMatches("<div></div>");
        }

        [Fact]
        public void Test_CreateComponent_With_Parameter()
        {
            TestContext.Render(builder =>
            builder.CreateComponent<CreateComponent>(0, attributes: new { Disabled = true }))
                .MarkupMatches("<div disabled=\"disabled\"></div>");
        }

        [Fact]
        public void Test_CreateComponent_With_Parameter_And_Class()
        {
            TestContext.Render(builder =>
            builder.CreateComponent<CreateComponent>(0, attributes: new { Disabled = true, @class = "my-class" }))
                .MarkupMatches("<div disabled=\"disabled\" class=\"my-class\"></div>");
        }

        [Fact]
        public void Test_CreateComponent_With_ChildContent()
        {
            TestContext.Render(builder =>
            builder.CreateComponent<CreateComponent>(0, "test"))
                .MarkupMatches("<div>test</div>");

            TestContext.Render(builder =>
            builder.CreateComponent<CreateComponent>(0, content => content.CreateElement(0, "span", "test")))
                .MarkupMatches("<div><span>test</span></div>");
        }
    }

    class CreateComponent : BlazorAbstractComponentBase, IHasChildContent
    {
        [Parameter][HtmlAttribute("disabled")] public bool Disabled { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
