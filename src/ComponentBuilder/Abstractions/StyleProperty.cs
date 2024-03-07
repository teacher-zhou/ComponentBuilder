namespace ComponentBuilder;

/// <summary>
/// Generate css style.
/// </summary>
public class StyleProperty : Dictionary<string, object?>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public StyleProperty()
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public StyleProperty(IDictionary<string, object?> dictionary) : base(dictionary)
    {
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public StyleProperty(IEnumerable<KeyValuePair<string, object?>> collection) : base(collection)
    {
    }

    /// <summary>
    /// Convert style string with semi-comma(;) for each item.
    /// </summary>
    public override string ToString() => string.Join(";", this.Where(m => m.Value switch
    {
        bool value => value,
        object value => value is not null
    }).Select(m => $"{m.Key}:{m.Value}"));

    /// <summary>
    /// Convert style string(key1:value1;key2:value2...) to <see cref="StyleProperty"/> class.
    /// </summary>
    public static implicit operator StyleProperty(string? styleString)
    {
        ArgumentException.ThrowIfNullOrEmpty(styleString, nameof(styleString));

        var dic = styleString.Trim().Split(';').Select(key =>
        {
            var name = key.Split(":")[0].Trim();
            var value = key.Split(":")[1].Trim();

            return new KeyValuePair<string, object?>(name, value);
        });
        return new(dic);
    }
}
