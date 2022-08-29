using System.Linq;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// 默认 <see cref="ICssClassBuilder"/> 的实现。
/// </summary>
public class DefaultCssClassBuilder : ICssClassBuilder
{

    private readonly IList<string> _classes;

    /// <summary>
    /// 初始化 <see cref="DefaultCssClassBuilder"/> 类的新实例。
    /// </summary>
    public DefaultCssClassBuilder() => _classes = new List<string>();

    /// <summary>
    /// 获取 CSS 列表。
    /// </summary>
    public IEnumerable<string> CssList => _classes;

    /// <inheritdoc/>
    public ICssClassBuilder Append(string value)
    {
        if (!string.IsNullOrEmpty(value) && !_classes.Contains(value))
        {
            _classes.Add(value);
        }
        return this;
    }

    /// <summary>
    /// 用空格连接 CSS 字符串。
    /// </summary>
    public override string ToString() => string.Join(" ", _classes.Distinct());

    /// <summary>
    /// 清楚 CSS 列表。
    /// </summary>
    public void Clear() => _classes.Clear();

    /// <inheritdoc/>
    void IDisposable.Dispose() => Clear();

    /// <inheritdoc/>
    public ICssClassBuilder Insert(int index, string value)
    {
        if (!string.IsNullOrEmpty(value) && !_classes.Contains(value))
        {
            _classes.Insert(index, value);
        }
        return this;
    }
}
