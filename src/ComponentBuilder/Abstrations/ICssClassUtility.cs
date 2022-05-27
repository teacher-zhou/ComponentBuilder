namespace ComponentBuilder.Abstrations;

/// <summary>
/// This interface is used by component frameworks to extend common CSS class names.
/// </summary>
public interface ICssClassUtility
{
    /// <summary>
    /// Append specific CSS value to the builder.
    /// </summary>
    /// <param name="value">The value of CSS class string.</param>
    /// <returns>A <see cref="ICssClassUtility"/> instance including the value.</returns>
    ICssClassUtility Append(string value);
    /// <summary>
    /// Returns a seriers of CSS class names.
    /// </summary>
    IEnumerable<string> CssClasses { get; }
}
