namespace ComponentBuilder;

/// <summary>
/// Provides a CSS class builder.
/// </summary>
public interface ICssClassBuilder : IDisposable
{
    /// <summary>
    /// Append specified CSS value.
    /// </summary>
    /// <param name="value">Value of CSS string.</param>
    /// <returns>A <see cref="ICssClassBuilder"/> instacen including new value.</returns>
    ICssClassBuilder Append(string? value);

    /// <summary>
    /// Determines a bool value that container has specified CSS value.
    /// </summary>
    /// <param name="value">A CSS value to determine.</param>
    /// <returns><c>true</c> for containing value, otherwise, <c>false</c>.</returns>
    bool Contains(string? value);

    /// <summary>
    /// Returns a bool value indicating container is empty.
    /// </summary>
    /// <returns><c>true</c> is empty, otherwise, <c>false</c>.</returns>
    bool IsEmpty();

    /// <summary>
    /// Insert the CSS value into a specific index of the collection.
    /// </summary>
    /// <param name="index">The index to insert</param>
    /// <param name="value">CSS value to insert.</param>
    /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> is out of bound.</exception>
    /// <returns>A <see cref="ICssClassBuilder"/> instacen including new value.</returns>
    ICssClassBuilder Insert(int index, string? value);
    /// <summary>
    /// Removes the specified value from builder.
    /// </summary>
    /// <param name="value">A CSS value to remove.</param>
    /// <returns>A <see cref="ICssClassBuilder"/> instance removed value.</returns>
    ICssClassBuilder Remove(string? value);

    /// <summary>
    /// Converts the string to a CSS class and concatenates all values in this builder.
    /// </summary>
    /// <returns>A series of CSS string seperated by spaces for each item.</returns>
    string? ToString();
}
