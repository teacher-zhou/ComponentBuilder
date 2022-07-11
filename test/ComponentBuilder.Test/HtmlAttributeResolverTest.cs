using ComponentBuilder.Abstrations;


using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test
{
    public class HtmlAttributeResolverTest : TestBase
    {
        public HtmlAttributeResolverTest()
        {
        }


        [Fact]
        public void Given_Invoke_Resolve_When_Component_Input_Title_Then_Title_Has_Value()
        {
            TestContext.RenderComponent<ElementPropertyComponent>(
                p => p.Add(m => m.Title, "abc").Add(m => m.Href, "www.bing.com")
            )
            .Should().HaveAttribute("title", "abc")
                .And.HaveAttribute("href","www.bing.com");
        }

        [Fact]
        public void When_Enum_Has_HtmlAttribute_Then_Parameter_Is_Enum_To_Use_HtmlAttribute()
        {
            TestContext.RenderComponent<ElementPropertyComponent>(
                p => p.Add(m => m.Title, "abc").Add(m => m.Href, "www.bing.com").Add(m=>m.Target,ElementPropertyComponent.LinkTarget.Blank)
            )
            .Should().HaveAttribute("title", "abc")
                .And.HaveAttribute("href", "www.bing.com")
                .And.HaveAttribute("target","_blank")
                ;
        }

        [Fact]
        public void Given_Invoke_Resolve_When_Component_Drop_Is_Bool_Then_DataToggle_Has_Value_Of_Drop()
        {
            TestContext.RenderComponent<ElementPropertyComponent>(
                p => p.Add(m => m.Drop, true)
            ).Should().HaveAttribute("data-toggle", "drop");
        }



        [Fact]
        public void Given_Invoke_Resolve_When_Component_Drap_Has_Value_Then_Has_Key_DataDrag_With_Value_Yes()
        {
            TestContext.RenderComponent<ElementPropertyComponent>(
                p => p.Add(m => m.Drag, "drag")
            ).Should().HaveAttribute("data-drag", "drag");
        }

        [Fact]
        public void Given_Invoke_Resolve_When_Component_Height_Has_Value_Then_Has_Key_Height_With_Number_Value()
        {
            TestContext.RenderComponent<ElementPropertyComponent>(
                p => p.Add(m => m.Number, 100).Add(m=>m.Title,"height")
            ).Should().HaveAttribute("data-height", "100")
            .And.HaveAttribute("title","height")
            ;
        }

        [Fact]
        public void Given_Invoke_Resolve_When_Component_Auto_Is_Bool_Without_Name_Then_Has_Key_Is_Auto_With_Value_Auto()
        {
            TestContext.RenderComponent<ElementPropertyComponent>(
                p => p.Add(m => m.Auto, true)
            ).Should().HaveAttribute("data-auto", "auto");
        }
    }

    [HtmlTag("a")]
    class ElementPropertyComponent : BlazorComponentBase
    {
        [Parameter][HtmlAttribute("title")] public string Title { get; set; }
        [Parameter][HtmlAttribute] public string Href { get; set; }

        [Parameter][HtmlData("toggle")]public bool Drop { get; set; }

        [Parameter][HtmlAttribute] public LinkTarget? Target { get; set; }

        [Parameter][HtmlData]public string Drag { get; set; }

        [Parameter][HtmlData("height")]public int? Number { get; set; }

        [Parameter][HtmlData]public bool Auto { get; set; }

        public enum LinkTarget
        {
            [HtmlAttribute("_blank")]Blank,
            [HtmlAttribute("_self")]Self
        }
    }
}
