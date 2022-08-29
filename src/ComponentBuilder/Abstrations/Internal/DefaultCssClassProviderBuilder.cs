namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// <see cref="ICssClassProvider"/> 默认实现。
/// </summary>
internal class DefaultCssClassProviderBuilder : ICssClassProvider
{
    private readonly ICollection<string> _classes = new List<string>();

    /// <summary>
    /// 初始化 <see cref="DefaultCssClassProviderBuilder"/> 类的新实例。
    /// </summary>
    public DefaultCssClassProviderBuilder()
    {
    }

    /// <summary>
    /// Gets the css class list appended.
    /// </summary>
    public IEnumerable<string> CssClasses => _classes;

    /// <inheritdoc/>
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
