namespace ComponentBuilder.Abstrations;

/// <summary>
/// Provides utilities of css class.
/// </summary>
public interface ICssClassUtility
{
    /// <summary>
    /// Appends to build css class utility.
    /// </summary>
    /// <param name="value">A string value to append.</param>
    /// <returns>The instance implementation from <see cref="ICssClassUtility"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    ICssClassUtility Append(string value);
    /// <summary>
    /// Returns css class list appended.
    /// </summary>
    IEnumerable<string> CssClasses { get; }
}
