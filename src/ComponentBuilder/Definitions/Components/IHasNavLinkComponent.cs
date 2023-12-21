using Microsoft.AspNetCore.Components.Routing;

namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides navigation links for components.
/// </summary>
public interface IHasNavLinkComponent : IHasChildContent
{
    /// <summary>
    /// Gets or sets the behavior of a matching navigation link.
    /// </summary>
    NavLinkMatch Match { get; set; }

    /// <summary>
    /// Gets a Boolean value indicating that the link matches the url.
    /// </summary>
    bool IsActive { get; set; }

    /// <summary>
    /// Gets the CSS string when the uri link matches.
    /// </summary>
    string? ActiveCssClass => "active";
}
