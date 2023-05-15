using ComponentBuilder;
using ComponentBuilder.Definitions;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Demo.ServerSide.Components;

/// <summary>
/// 表示导航栏容器的项。
/// <para>
/// 要求在 <see cref="NavbarNavigator"/> 组件中使用。
/// </para>
/// </summary>
[HtmlTag("li")]
[CssClass("nav-item")]
public class NavItem : BlazorComponentBase, IHasChildContent
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
