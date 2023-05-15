using ComponentBuilder.Automation.Builder;
using ComponentBuilder.Automation.Definitions;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Automation.Test
{
    public class CssClassUtilityTest : TestBase
    {
        [Fact]
        public void When_Invoke_Css_Class_To_Append_Value_Then_Append_CssClass_Behind_Configurations()
        {
            TestContext.RenderComponent<CssClassUtilityComponent>(p =>
            {
                p.Add(m => m.Color, "primary")
                .Add(m => m.CssClass, Css.Class.Append("css"));
            }).Should().HaveClass("color-primary")
            .And.HaveClass("css");
        }

        [Fact]
        public void When_Invoke_Css_Class_By_Extensions_Then_Append_CssClass_Behind()
        {
            TestContext.RenderComponent<CssClassUtilityComponent>(p =>
            {
                p.Add(m => m.Color, "primary")
                .Add(m => m.CssClass, Css.Class.Visibility().Hide().Show());
            }).Should().HaveClass("color-primary")
            .And.HaveClass("visible")
            .And.HaveClass("hide")
            .And.HaveClass("show")
            ;
        }


        [Fact]
        public void Given_Mulitiple_Components_When_Invoke_Css_Class_By_Extensions_Then_Append_CssClass_Behind()
        {
            TestContext.RenderComponent<CssClassUtilityComponent>(p =>
            {
                p.Add(m => m.Color, "primary")
                .Add(m => m.CssClass, Css.Class.Visibility())
                ;
            }).Should().HaveClass("color-primary")
            .And.HaveClass("visible")
            ;


            TestContext.RenderComponent<CssClassUtilityComponent>(p =>
            {
                p
                .Add(m => m.CssClass, Css.Class.Show());
            }).Should().HaveClass("show")
            ;

            TestContext.RenderComponent<CssClassUtilityComponent>(p =>
            {
                p.Add(m => m.Color, "primary")
                .Add(m => m.CssClass, Css.Class.Visibility().Show());
            }).Should().HaveClass("color-primary")
            .And.HaveClass("visible")
            .And.HaveClass("show")
            ;

            TestContext.RenderComponent<CssClassUtilityComponent>(p =>
            {
                p.Add(m => m.Color, "primary")
                .Add(m => m.CssClass, Css.Class.Visibility().Hide());
            }).Should().HaveClass("color-primary")
           .And.HaveClass("visible")
           .And.HaveClass("hide")
           ;
        }
    }

    class CssClassUtilityComponent : BlazorComponentBase,IHasCssClassUtility
    {
        [Parameter][CssClass("color-")] public string Color { get; set; }
        [Parameter]public ICssClassUtility? CssClass { get; set; }
    }

    static class ClassUtility
    {
        public static ICssClassUtility Visibility(this ICssClassUtility css) => css.Append("visible");
        public static ICssClassUtility Hide(this ICssClassUtility css) => css.Append("hide");

        public static ICssClassUtility Show(this ICssClassUtility css) => css.Append("show");
    }
}
