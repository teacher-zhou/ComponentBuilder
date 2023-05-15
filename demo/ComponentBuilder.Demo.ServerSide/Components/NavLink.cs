using ComponentBuilder;
using ComponentBuilder.Definitions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Demo.ServerSide.Components;

/// <summary>
/// 表示导航项的超链接组件。该组件会根据页面的路由自动设置 <see cref="IHasNavLinkComponent.ActiveCssClass"/> 的高亮样式。
/// <para>
/// 要求在 <see cref="NavbarNavigator"/> 组件中使用。
/// </para>
/// </summary>
[HtmlTag("a")]
[CssClass("nav-link")]
public class NavAnchor : BlazorComponentBase, IHasNavLinkComponent
{
    /// <summary>
    /// 超链接字符串。
    /// </summary>
    [Parameter][HtmlAttribute("href")] public string? Link { get; set; }
    /// <inheritdoc/>
    [Parameter] public NavLinkMatch Match { get; set; }
    /// <inheritdoc/>
    public bool IsActive { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
