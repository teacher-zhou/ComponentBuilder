namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// Default class for <see cref="ICssClassProvider"/> implementation.
/// </summary>
internal class DefaultCssClassProviderBuilder : ICssClassProvider
{
    private readonly ICollection<string> _classes = new List<string>();

    /// <summary>
    /// Initializes a new instance of <see cref="DefaultCssClassProviderBuilder"/> class.
    /// </summary>
    public DefaultCssClassProviderBuilder()
    {
    }

    /// <summary>
    /// Gets the css class list appended.
    /// </summary>
    public IEnumerable<string> CssClasses => _classes;

    /// <summary>
    /// Appends css class string to list.
    /// </summary>
    /// <param name="value">css class string.</param>
    /// <returns>Current instance implemented from <see cref="ICssClassProvider"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="value"/> cannot be null or whitespace.</exception>
    public ICssClassProvider Append(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
        }

        _classes.Add(value);
        return this;
    }
}
