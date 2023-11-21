namespace ComponentBuilder;

/// <summary>
/// Provides a container for generating styles.
/// </summary>
public interface IStyleBuilder
{
    /// <summary>
    /// Append a new style value to the generator.
    /// </summary>
    /// <param name="value">The style value.</param>
    IStyleBuilder Append(string? value);
    /// <summary>
    /// Clear all styles.
    /// </summary>
    void Clear();
    /// <summary>
    /// Converts a string to a style and concatenates all values in this builder.
    /// </summary>
    /// <returns>Each entry is composed of semicolons (;) A delimited series of strings.</returns>
    string ToString();
}
