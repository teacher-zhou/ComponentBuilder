using ComponentBuilder.Definitions;
using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Rendering;
/// <summary>
/// 实现 <see cref="IHasNavLinkComponent"/> 接口的组件将渲染成 <see cref="NavLink"/> 组件。
/// </summary>
public class NavLinkComponentRender : IComponentRender
{
    /// <inheritdoc/>
    public bool Render(IBlazorComponent component, RenderTreeBuilder builder)
    {
        if ( component is IHasNavLinkComponent navLink )
        {
            builder.OpenComponent<NavLink>(0);
            builder.AddAttribute(1, nameof(NavLink.Match), navLink.Match);
            builder.AddAttribute(2, nameof(NavLink.ActiveClass), navLink.ActiveCssClass);
            builder.AddAttribute(3, nameof(NavLink.ChildContent), navLink.ChildContent);
            builder.AddMultipleAttributes(4, component.GetAttributes());
            builder.CloseComponent();
            return false;
        }
        return true;
    }
}
