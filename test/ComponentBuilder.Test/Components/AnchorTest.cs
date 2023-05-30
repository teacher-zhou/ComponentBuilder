using ComponentBuilder.Definitions;

namespace ComponentBuilder.Test.Components;
public class AnchorTest:AutoTestBase
{
    [Fact]
    public void Given_Anchor_Implement_AnchorComponent_Interface()
    {

/* Unmerged change from project 'ComponentBuilder.Test (net7.0)'
Before:
        TestContext.RenderComponent<Anchor>(m => m.Add(p => p.Href, "www.bing.com").Add(p => p.Target, Parameters.AnchorTarget.Blank))
After:
        TestContext.RenderComponent<Anchor>(m => m.Add(p => p.Href, "www.bing.com").Add(p => p.Target, ComponentBuilder.Definitions.Components.AnchorTarget.Blank))
*/
        TestContext.RenderComponent<Anchor>(m => m.Add(p => p.Href, "www.bing.com").Add(p => p.Target, AnchorTarget.Blank))
            .Should().HaveTag("a").
            And.HaveAttribute("href", "www.bing.com")
            .And.HaveAttribute("target", "_blank")
            .And.HaveAttribute("role", "alert");
    }
}
