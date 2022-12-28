using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides a component has navigation link.
/// </summary>
public interface IHasNavLink : IComponent
{
    /// <summary>
    /// Gets the navigation manage.
    /// </summary>
    NavigationManager NavigationManager { get; }

    /// <summary>
    /// Gets or sets the behavior of nav link can be matched.
    /// </summary>
    NavLinkMatch Match { get; set; }
}
