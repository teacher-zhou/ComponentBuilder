namespace ComponentBuilder.Abstrations.Internal;

public class DefaultStyleBuilder : IStyleBuilder
{
    private readonly ICollection<string> _styles;
    public DefaultStyleBuilder()
    {
        _styles = new List<string>();
    }

    public IStyleBuilder Append(string value)
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

    void IDisposable.Dispose() => Clear();

    public override string ToString()
    => string.Join(";", _styles);
}
