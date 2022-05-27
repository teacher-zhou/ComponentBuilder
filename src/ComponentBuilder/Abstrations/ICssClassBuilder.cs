namespace ComponentBuilder.Abstrations;

/// <summary>
/// Defines a CSS string builder of component.
/// </summary>
public interface ICssClassBuilder : IDisposable
{
    /// <summary>
    /// Append specific CSS value to the builder.
    /// </summary>
    /// <param name="value">The value of CSS class string.</param>
    /// <returns>A <see cref="ICssClassBuilder"/> instance including the value.</returns>
    ICssClassBuilder Append(string value);

    /// <summary>
    /// Insert CSS value into specific index of collection in this builder.
    /// </summary>
    /// <param name="index">The index to insert.</param>
    /// <param name="value">The value of CSS class string.</param>
    /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> is out of range.</exception>
    /// <returns>A <see cref="ICssClassBuilder"/> instance including the value.</returns>
    ICssClassBuilder Insert(int index, string value);

    /// <summary>
    /// Convert string to CSS class witch concat all values in this builder.
    /// </summary>
    /// <returns>A string representing CSS class seperated by space for each item.</returns>
    string ToString();
}
