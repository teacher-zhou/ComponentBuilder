namespace ComponentBuilder;

/// <summary>
/// Provides a container to build style.
/// </summary>
public interface IStyleBuilder : IDisposable
{
    /// <summary>
    /// Append a new style value to builder.
    /// </summary>
    /// <param name="value">value of style.</param>
    IStyleBuilder Append(string? value);
    /// <summary>
    /// Converts the string to style and concatenates all values in this builder.
    /// </summary>
    /// <returns>A series of string seperated by semi-colon(;) for each item.</returns>
    string ToString();
}
