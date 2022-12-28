using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace ComponentBuilder.Test
{
    public class RenderTreeBuilderExtensionTest : TestBase
    {
        [Fact]
        public void Test_CreateElement()
        {
            TestContext.Render(builder => builder.CreateElement(0, "div", "abc"))
                .MarkupMatches("<div>abc</div>");

            TestContext.Render(builder => builder.CreateElement(1,"div", childContent =>
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
            builder.CreateComponent<CreateComponent>(0, attributes: new { Disabled=true,@class="my-class" }))
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

        [Fact]
        public void Given_CreateElement_When_Add_Same_Name_Of_Attribute_Then_Take_First()
        {
            TestContext.Render(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "c1");
                builder.AddAttribute(2, "class", "c2");
                builder.AddAttribute(3, "class", "c3");
                builder.CloseElement();
            }).MarkupMatches("<div class=\"c1\"></div>");

            TestContext.Render(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "disabled", true);
                builder.AddAttribute(2, "disabled", false);
                builder.CloseElement();
            }).MarkupMatches("<div disabled></div>");
        }

        [Fact]
        public void Given_CreateElement_When_Add_Multiple_Content_Then_Combine_Them()
        {
            TestContext.Render(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddContent(1, "hello");
                builder.AddContent(2, "world");
                builder.AddContent(3, "go");
                builder.AddContent(4, "c#");
                builder.CloseElement();
            }).MarkupMatches("<div>helloworldgoc#</div>");
        }

        [Fact]
        public void Test_Capture_Reference_When_Create_Element()
        {
            ElementReference? element = null;
            TestContext.Render(builder =>
            {
                builder.CreateElement(0, "div", "content", captureReference: el => element = el);
            });
            Assert.NotNull(element);
        }


        [Fact]
        public void Test_Capture_Reference_When_Create_Component()
        {
            object? component = null;
            TestContext.Render(builder =>
            {
                builder.CreateComponent<CreateComponent>(0,  captureReference: el => component = el);
            });
            Assert.NotNull(component);
        }


    }

    class CreateComponent : BlazorComponentBase, IHasChildContent
    {
        [Parameter][HtmlAttribute("disabled")] public bool Disabled { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
