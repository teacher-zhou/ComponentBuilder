using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Rendering;
/// <summary>
/// Components that implement the <see cref="IHasNavLinkComponent"/> interface are rendered as <see cref="NavLink"/> components.
/// </summary>
public class NavLinkComponentRender : IComponentRenderer
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
            builder.AddMultipleAttributes(4, component.GetAttributes()!);
            builder.CloseComponent();
            return false;
        }
        return true;
    }
}
