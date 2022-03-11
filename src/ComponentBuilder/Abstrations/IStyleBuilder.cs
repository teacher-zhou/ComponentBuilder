namespace ComponentBuilder.Abstrations;

/// <summary>
/// Prodive a builder to create style.
/// </summary>
public interface IStyleBuilder:IDisposable
{
    /// <summary>
    /// Appends style value.
    /// </summary>
    /// <param name="value">The value of style.</param>
    /// <returns>An instance of <see cref="IStyleBuilder"/>.</returns>
    IStyleBuilder Append(string value);
    /// <summary>
    /// Convert to style string sperated by ';' char.
    /// </summary>
    /// <returns>A string represents html element style.</returns>
    string ToString();
}
