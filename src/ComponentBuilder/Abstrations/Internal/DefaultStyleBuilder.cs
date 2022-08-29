namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// 默认的 <see cref="IStyleBuilder"/> 实现。
/// </summary>
public class DefaultStyleBuilder : IStyleBuilder
{
    private readonly ICollection<string> _styles;
    /// <summary>
    /// 初始化 <see cref="DefaultStyleBuilder"/> 类的新实例。
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

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    void IDisposable.Dispose() => Clear();

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns></returns>
    public override string ToString() => string.Join(";", _styles);
}
