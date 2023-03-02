using ComponentBuilder.Definitions;
using ComponentBuilder.Definitions.Parameters;
using ComponentBuilder.Fluent;
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
                builder.Fluent().Element("div")
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
                builder.Fluent().Component<FluentTreeComponent>()
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
                builder.Fluent().Element("div")
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
        public void Test_MultipleAttributes()
        {
            TestContext.Render(builder =>
            {
                builder.Fluent().Element("div")
                        .MultipleAttributes(new { id = "#id", data_toggle = "collapse", placeholder = "space" })
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "id", "#id");
                builder.AddAttribute(2, "data-toggle", "collapse");
                builder.AddAttribute(3, "placeholder", "space");
                builder.AddContent(10, "text");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Given_Attribute_Has_Same_Key_When_Value_Is_Not_String_Then_Last_Value_Should_Be_Get()
        {
            TestContext.Render(builder =>
            {
                builder.Fluent().Element("div")
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
        public void Test_Attribute_With_Anonymous()
        {
            TestContext.Render(builder =>
            {
                builder.Fluent().Element("div")
                        .Attribute(new { count = 3, placeholder = "space", disabled = true })
                        .Content("text")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "count", 3);
                builder.AddAttribute(2, "placeholder", "space");
                builder.AddAttribute(3, "disabled", true);
                builder.AddContent(10, "text");
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Create_Any_Way_Of_Builder()
        {
            TestContext.Render(builder =>
            {
                builder.Fluent().Element("div")
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
                builder.Fluent().Element("div")
                        .Attribute("class", "value")
                        .Content(child => child.Fluent().Element("h1").Content("header").Close())
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
                builder.Fluent().Component<FluentTreeComponent>()
                        .Attribute("class", "value")
                        .Content(child => child.Fluent().Element("h1").Content("header").Close())
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
        public void Test_Create_Child_Content_WithComponent_NestedChildComponent()
        {
            TestContext.Render(builder =>
            {
                builder.Fluent().Component<FluentTreeComponent>()
                        .Attribute("class", "value")
                        .Attribute("ChildContent", HtmlHelper.CreateContent(child => child.Fluent().Component<FluentTreeComponent>().Attribute("ChildContent", HtmlHelper.CreateContent("header")).Close()))
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenComponent<FluentTreeComponent>(0);
                builder.AddAttribute(1, "class", "value");
                builder.AddAttribute(2, "ChildContent", (RenderFragment)(child =>
                {
                    child.OpenComponent<FluentTreeComponent>(0);
                    child.AddAttribute(1, "ChildContent", HtmlHelper.CreateContent("header"));
                    child.CloseElement();
                }));
                builder.CloseElement();
            });
        }

        [Fact]
        public void Test_Capture_Ref_For_Component()
        {
            FluentTreeComponent? component = default;
            TestContext.Render(builder =>
            {
                builder.Fluent().Component<FluentTreeComponent>()
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
                builder.Fluent().Element("span")
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
                builder.Fluent().Element("span")
                        .Class("margin-1 padding-3")
                       .Close();
            }).MarkupMatches(builder =>
            {
                builder.OpenElement(0, "span");
                builder.AddAttribute(1, "class", "margin-1 padding-3");
                builder.CloseElement();
            });

            TestContext.Render(builder =>
            {
                builder.Fluent().Element("span")
                        .Class("margin-1 padding-3")
                        .Class("css fly")
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
                builder.Fluent().Element("span")
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
                builder.Fluent().Element("span")
                        .Style("width:100px;height:30px;")
                        .Style("opacity:0.5;")
                        .Style(("text-decoration:underline"))
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
                builder.Fluent().Element("div")
                        .Callback("onclick", HtmlHelper.Event.Create(this, () =>
                        {
                            count++;
                        }))
                        .Close();
            });

            component.Find("div").Click();

            Assert.NotEqual(0, count);
        }

        [Fact]
        public void Test_Fluent_Element()
        {
            TestContext.Render(b =>
            {
                b.Fluent().Span().Content("text1").Close()
                .Span().Content("text2").Close()
                .Span().Content("text3").Close();
            }).MarkupMatches(b =>
            {
                b.CreateElement(0, "span", "text1");
                b.CreateElement(0, "span", "text2");
                b.CreateElement(2, "span", "text3");
            });
        }

        [Theory]
        [InlineData(new object[] { true })]
        [InlineData(new object[] { false })]
        public void Test_Condition_Fluent(bool flag)
        {
            TestContext.Render(b =>
            {
                b.Fluent().Div("nav", flag).Content("test").Close();
            }).MarkupMatches(b =>
            {
                if (flag)
                {
                    b.OpenElement(0, "div");
                    b.AddAttribute(1, "class", "nav");
                    b.AddContent(2, "test");
                    b.CloseElement();
                }
            });
        }

        [Fact]
        public void Test_ForEach_Root_Element()
        {
            TestContext.Render(b =>
            {
                b.Fluent().ForEach("div", 3);
            }).MarkupMatches(b =>
            {
                b.CreateElement(0, "div");
                b.CreateElement(1, "div");
                b.CreateElement(2, "div");
            });

            TestContext.Render(b =>
            {
                b.Fluent().ForEach("div", 3, result => result.attribute.Content("test"));
            }).MarkupMatches(b =>
            {
                b.CreateElement(0, "div", "test");
                b.CreateElement(1, "div", "test");
                b.CreateElement(2, "div", "test");
            });
        }

        [Fact]
        public void Test_ForEach_Child_Element()
        {
            TestContext.Render(b =>
            {
                b.Fluent().Ul().Content(builder => builder.Fluent().ForEach("li", 3)).Close();
            }).MarkupMatches(b =>
            {
                b.CreateElement(0, "ul", content =>
                {
                    b.CreateElement(0, "li");
                    b.CreateElement(1, "li");
                    b.CreateElement(2, "li");
                });
            });
        }

        [Fact]
        public void Test_Content_Without_Element()
        {
            TestContext.Render(b => b.Fluent().Content("text").Div().Content("div").Close().Content("behind"))
                .MarkupMatches(b =>
                {
                    b.AddContent(0, "text");
                    b.CreateElement(1, "div", "div");
                    b.AddContent(2, "behind");
                });
        }
    }



    [HtmlTag("a")]
    [ParentComponent]
    class FluentTreeComponent : BlazorComponentBase, IHasChildContent
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }
}
