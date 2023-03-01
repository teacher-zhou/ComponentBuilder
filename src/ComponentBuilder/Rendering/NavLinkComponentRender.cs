using ComponentBuilder.Definitions;
using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Rendering;
/// <summary>
/// A renderer can regonize <see cref="IHasNavLink"/> to render <see cref="NavLink"/> component.
/// </summary>
public class NavLinkComponentRender : IComponentRender
{
    /// <inheritdoc/>
    public bool Render(IBlazorComponent component, RenderTreeBuilder builder)
    {
        if ( component is IHasNavLink navLink )
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
