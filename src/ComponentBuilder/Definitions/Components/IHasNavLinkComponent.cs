using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Definitions;

/// <summary>
/// 提供组件的导航链接。
/// </summary>
public interface IHasNavLinkComponent : IHasChildContent
{
    /// <summary>
    /// 获取或设置可匹配的导航链接的行为。
    /// </summary>
    NavLinkMatch Match { get; set; }

    /// <summary>
    /// 获取一个布尔值，该值指示链接与url匹配。
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// 获取当 uri 链接匹配的 CSS 字符串。
    /// </summary>
    string? ActiveCssClass => "active";
}
