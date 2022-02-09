namespace ComponentBuilder.Abstrations;

/// <summary>
/// Provides a css class builder.
/// </summary>
public interface ICssClassBuilder : IDisposable
{
    /// <summary>
    /// Append a value to builder.
    /// </summary>
    /// <param name="value">A value to append.</param>
    /// <returns>The instance of <see cref="ICssClassBuilder"/>.</returns>
    ICssClassBuilder Append(string value);

    /// <summary>
    /// Insert specified value to specified index of collection
    /// </summary>
    /// <param name="index">The index to insert.</param>
    /// <param name="value">The value to insert.</param>
    /// <returns>The instance of <see cref="ICssClassBuilder"/>.</returns>
    /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> is out of range.</exception>
    ICssClassBuilder Insert(int index, string value);

    /// <summary>
    /// Convert css class in container to string.
    /// </summary>
    /// <returns>A string separated by space for each item.</returns>
    string ToString();
}
