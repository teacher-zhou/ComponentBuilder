using ComponentBuilder.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ComponentBuilder.Demo.ServerSide.Components;
[HtmlTag("button")]
public class AnimationButton : BlazorAbstractComponentBase, IHasChildContent, IHasOnClick
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<MouseEventArgs?> OnClick { get; set; }

    bool Clicked;
    protected override void BuildCssClass(ICssClassBuilder builder)
    {
        builder.Append("fade-out");

        if (builder.Contains("fade-in"))
        {
            builder.Remove("fade-out-active");
        }
        if (builder.Contains("fade-out"))
        {
            builder.Remove("fade-in-active");
        }

    }

    protected override void BuildAttributes(IDictionary<string, object> attributes)
    {
        attributes["onclick"] = HtmlHelper.Event.Create<MouseEventArgs?>(this, async e => await ClickButton());
    }

    public async Task ClickButton()
    {
        Clicked = true;
        await OnClick.InvokeAsync();
        if (CssClassBuilder.Contains("fade-out"))
        {
            CssClassBuilder.Append("fade-out-active").Remove("fade-in-active").Remove("fade-out").Append("fade-in");
        }
        else if (CssClassBuilder.Contains("fade-in"))
        {
            CssClassBuilder.Append("fade-in-active").Remove("fade-out-active").Remove("fade-in").Append("fade-out");
        }
        Clicked = true;
        StateHasChanged();
    }
}
