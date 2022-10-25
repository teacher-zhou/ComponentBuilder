namespace ComponentBuilder.Abstrations;

/// <summary>
/// Provides an interface to extend CSS class by extension methods.
/// </summary>
public interface ICssClassUtility
{
    /// <summary>
    /// Append a new value of CSS class string.
    /// </summary>
    /// <param name="value">A value of CSS class string to append.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
    ICssClassUtility Append(string value);
    /// <summary>
    /// Gets a series of CSS class string seperated by spaces.
    /// </summary>
    IEnumerable<string>? CssClasses { get; }
}
