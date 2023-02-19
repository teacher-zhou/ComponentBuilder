using ComponentBuilder.Fluent;
using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test
{
    public class FluentRenderTreeTest : TestBase
    {
        [Fact]
        public void Test_Create_Element_Compare_With_RenderTreeBuilder()
        {
            TestContext.Render(builder =>
            {
                builder.Element("div")
                        .Attribute("class", "value")
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "value");
                builder.AddContent(2, "text");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Create_Component_Compare_With_RenderTreeBuilder()
        {
            TestContext.Render(builder =>
            {
                builder.Component<FluentTreeComponent>()
                        .Attribute("class", "value")
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenComponent<FluentTreeComponent>(0);
                builder.AddAttribute(1, "class", "value");
                builder.AddContent(2, "text");
                builder.CloseComponent();
            });
        }

        [Fact]
        public void Given_Attribute_Has_Same_Key_Attribute_When_Add_Value_Is_String_Then_All_Value_Of_Attribute_Should_Be_Concat_Values()
        {
            TestContext.Render(builder =>
            {
                builder.Element("div")
                        .Attribute("class", "margin-top-3 ")
                        .Attribute("class", "padding-bottom-2 ")
                        .Attribute("class", "shadow-3")
                        .Attribute("placeholder", "space")
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "margin-top-3 padding-bottom-2 shadow-3");
                builder.AddAttribute(2, "placeholder", "space");
                builder.AddContent(2, "text");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Given_Attribute_Has_Same_Key_When_Value_Is_Not_String_Then_Last_Value_Should_Be_Get()
        {
            TestContext.Render(builder =>
            {
                builder.Element("div")
                        .Attribute("disabled", true)
                        .Attribute("disabled", false)
                        .Attribute("count", 3)
                        .Attribute("placeholder", "space")
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "count", 3);
                builder.AddAttribute(2, "placeholder", "space");
                builder.AddContent(2, "text");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Create_Any_Way_Of_Builder()
        {
            TestContext.Render(builder =>
            {
                builder.Element("div")
                        .Attribute("class", "value")
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "value");
                builder.AddContent(2, "text");
                builder.CloseElement();
            });
        }
        [Fact]
        public void Test_Create_Child_Content()
        {
            TestContext.Render(builder =>
            {
                builder.Element("div")
                        .Attribute("class", "value")
                        .Content(child => child.Element("h1").Content("header").Close())
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "value");
                builder.AddContent(2, child =>
                {
                    child.OpenElement(0, "h1");
                    child.AddContent(1, "header");
                    child.CloseElement();
                });
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Create_Child_Content_WithComponent()
        {
            TestContext.Render(builder =>
            {
                builder.Component<FluentTreeComponent>()
                        .Attribute("class", "value")
                        .Content(child => child.Element("h1").Content("header").Close())
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenComponent<FluentTreeComponent>(0);
                builder.AddAttribute(1, "class", "value");
                builder.AddContent(2, child =>
                {
                    child.OpenElement(0, "h1");
                    child.AddContent(1, "header");
                    child.CloseElement();
                });
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Capture_Ref_For_Component()
        {
            FluentTreeComponent? component = default;
            TestContext.Render(builder =>
            {
                builder.Component<FluentTreeComponent>()
                        .Ref<FluentTreeComponent>(el => component = el)
                       .Close();
            });

            Assert.NotNull(component);
        }

        [Fact]
        public void Test_Capture_Ref_For_Element()
        {
            ElementReference? element = default;
            TestContext.Render(builder =>
            {
                builder.Element("span")
                        .Ref(el => element = el)
                       .Close();
            });

            Assert.NotNull(element);
        }

        [Fact]
        public void Test_Class_Extension()
        {
            TestContext.Render(builder =>
            {
                builder.Element("span")
                        .Class("margin-1", "padding-3")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "margin-1 padding-3");
                builder.CloseElement();
            });

            TestContext.Render(builder =>
            {
                builder.Element("span")
                        .Class("margin-1", "padding-3")
                        .Class("css", "fly")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "margin-1 padding-3 css fly");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Style_Extension()
        {
            TestContext.Render(builder =>
            {
                builder.Element("span")
                        .Style("width:100px")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "style", "width:100px");
                builder.CloseElement();
            });

            TestContext.Render(builder =>
            {
                builder.Element("span")
                        .Style("width:100px", "height:30px")
                        .Style("opacity:0.5")
                        .Style(("text-decoration", "underline"))
                    .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "style", "width:100px;height:30px;opacity:0.5;text-decoration:underline");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_EventCallback()
        {
            int count = 0;

            var component = TestContext.Render(builder =>
            {
                builder.Element("div")
                        .Callback("onclick", HtmlHelper.Event.Create(this, () =>
                        {
                            count++;
                        }))
                        .Close();
            });

            component.Find("div").Click();

            Assert.NotEqual(0, count);
        }
    }



    [HtmlTag("a")]
    class FluentTreeComponent : BlazorComponentBase, IHasChildContent
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
