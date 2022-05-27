namespace ComponentBuilder;

/// <summary>
/// Define automatically blazor component.
/// </summary>
public interface IBlazorComponent : IComponent
{
    /// <summary>
    /// Returns a string for CSS attribute in element.
    /// </summary>
    /// <returns>A string separated by space for each item.</returns>
    string? GetCssClassString();
    /// <summary>
    /// Returns a string for style attribute in element.
    /// </summary>
    /// <returns>A string separated by ';' for each item.</returns>
    string? GetStyleString();
}
