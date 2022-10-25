namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// <see cref="ICssClassUtility"/> default implementation.
/// </summary>
internal class DefaultCssClassUtilityBuilder : ICssClassUtility
{
    private readonly ICollection<string> _classes = new List<string>();

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCssClassUtilityBuilder"/> class.
    /// </summary>
    public DefaultCssClassUtilityBuilder()
    {
    }

    /// <summary>
    /// Gets the css class list appended.
    /// </summary>
    public IEnumerable<string> CssClasses => _classes;

    /// <inheritdoc/>
    public ICssClassUtility Append(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"'{nameof(value)}' cannot be null or whitespace.", nameof(value));
        }

        _classes.Add(value);
        return this;
    }
}
