namespace ComponentBuilder.Abstrations;

/// <summary>
/// Defines a style string builder of component.。
/// </summary>
public interface IStyleBuilder : IDisposable
{
    /// <summary>
    /// Append specific style value to the builder.
    /// </summary>
    /// <param name="value">The value of style string.</param>
    /// <returns>A <see cref="IStyleBuilder"/> instance including the value.</returns>
    IStyleBuilder Append(string value);
    /// <summary>
    /// Convert string to style witch concat all values in this builder.
    /// </summary>
    /// <returns>A string representing style seperated by semicolon(;) for each item.</returns>
    string ToString();
}
