namespace ComponentBuilder.FluentClass;
/// <summary>
/// A provider that provides smooth CSS classes from parameters.
/// </summary>
public interface IFluentClassProvider
{
    /// <summary>
    /// Create a series of strings for the CSS class.
    /// </summary>
    /// <returns>A collection of strings representing a CSS class.</returns>
    IEnumerable<string> Create();
}
