using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Abstrations;

/// <summary>
/// 表示当前组件可以作为超链接组件使用，并探测路由是否与当前的 href 链接匹配。
/// </summary>
public interface IAnchorComponent
{
    /// <summary>
    /// 获取或设置路由导航对于超链接的匹配方式。
    /// </summary>
    NavLinkMatch Match { get; set; }
    /// <summary>
    /// 获取一个布尔值，表示路由匹配当前组件定义的超链接。
    /// </summary>
    public bool IsActive { get; }
}
