using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Test;

public class EnumerationTest:TestBase
{
    [Fact]
    public void CreateColorWithButton()
    {
        TestContext.RenderComponent<Button>(p=>p.Add(m=>m.Color,Color.Primary))
            .Should().HaveClass("btn-Primary");
    }

    [HtmlTag("button")]
    class Button : BlazorAbstractComponentBase
    {
        [Parameter][CssClass("btn-")]public Color? Color { get; set; }
    }

    class Color : Enumeration
    {
        internal Color(string value) : base(value)
        {
        }

        public static Color Primary = new(nameof(Primary));
        public static Color Secondary = new(nameof(Secondary));
        public static Color Info = new(nameof(Info));
    }
}
