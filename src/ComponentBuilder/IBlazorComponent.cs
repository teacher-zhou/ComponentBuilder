namespace ComponentBuilder;

/// <summary>
/// Define a blazor component.
/// </summary>
public interface IBlazorComponent : IComponent
{
    /// <summary>
    /// Returns a string of css attribute in the element
    /// </summary>
    /// <returns>A string separated by space for each item.</returns>
    string? GetCssClassString();
    /// <summary>
    /// Returns a string of style attribute in the element.
    /// </summary>
    /// <returns>A string separated by semi-colon for each item.</returns>
    string? GetStyleString();
}
