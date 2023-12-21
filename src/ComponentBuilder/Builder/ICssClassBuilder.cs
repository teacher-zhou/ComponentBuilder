namespace ComponentBuilder;

/// <summary>
/// Provides a CSS class builder.
/// </summary>
public interface ICssClassBuilder
{
    /// <summary>
    /// Appends the specified CSS value.
    /// </summary>
    /// <param name="value">The value of the CSS string.</param>
    ICssClassBuilder Append(string? value);

    /// <summary>
    /// Determines that the specified CSS value is included.
    /// </summary>
    /// <param name="value">The value of the CSS string.</param>
    /// <returns><c>true</c> if the value is included, otherwise <c>false</c>.</returns>
    bool Contains(string? value);

    /// <summary>
    /// Returns a bool value indicating that the container is empty.
    /// </summary>
    /// <returns><c>true</c> if it is empty, otherwise <c>false</c>.</returns>
    bool IsEmpty();

    /// <summary>
    /// Inserts CSS values into a specific index of the collection.
    /// </summary>
    /// <param name="index">Index to be inserted.</param>
    /// <param name="value">The value to insert.</param>
    /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> is out of bound.</exception>
    ICssClassBuilder Insert(int index, string? value);
    /// <summary>
    /// Removes the specified value from the builder.
    /// </summary>
    /// <param name="value">The value to remove.</param>
    ICssClassBuilder Remove(string? value);

    /// <summary>
    /// Clear all data.
    /// </summary>
    void Clear();

    /// <summary>
    /// Converts a string into a CSS class and concatenates all the values in this builder.
    /// </summary>
    /// <returns>A series of CSS strings, each entry separated by a space.</returns>
    string? ToString();
}
