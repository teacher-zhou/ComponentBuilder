namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// Default <see cref="IStyleBuilder"/> implementation.
/// </summary>
internal class DefaultStyleBuilder : IStyleBuilder
{
    private readonly ICollection<string> _styles;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultStyleBuilder"/> class.
    /// </summary>
    public DefaultStyleBuilder()
    {
        _styles = new List<string>();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public IStyleBuilder Append(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return this;
        }

        _styles.Add(value);
        return this;
    }


    /// <summary>
    /// Clear css class string in container.
    /// </summary>
    public void Clear() => _styles.Clear();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => string.Join(";", _styles);
}
