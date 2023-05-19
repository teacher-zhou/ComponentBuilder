using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;
/// <summary>
/// 表示样式的选择器。
/// </summary>
public class StyleSelector
{
    readonly Dictionary<string, string> _selectors = new();

    /// <summary>
    /// 具有指定键和样式值的样式。
    /// </summary>
    /// <param name="key">CSS 选择器。</param>
    /// <param name="value">键对应的值。</param>
    /// <exception cref="ArgumentException"><paramref name="key"/> 是 null 或空字符串。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> 是 null。</exception>
    public StyleSelector Add(string key, StyleProperty value)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }

        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        key = key.Replace("_", "-");
        _selectors[key] = value.ToString();
        return this;
    }

    /// <summary>
    /// 具有指定键和样式值的样式。
    /// </summary>
    /// <param name="key">CSS 选择器。</param>
    /// <param name="value">键对应的值。</param>
    /// <exception cref="ArgumentException"><paramref name="key"/> 或 <paramref name="styleString"/> 是 null 或空字符串。</exception>
    public StyleSelector Add(string key, string styleString)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }

        if (string.IsNullOrEmpty(styleString))
        {
            throw new ArgumentException($"'{nameof(styleString)}' cannot be null or empty.", nameof(styleString));
        }
        key = key.Replace("_", "-");
        _selectors[key] = styleString;
        return this;
    }

    /// <summary>
    /// 以样式返回CSS区域字符串。
    /// </summary>
    public override string ToString()
    => _selectors.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev}\n{next}");
}

/// <summary>
/// 表示样式区域中CSS键/值对的集合。
/// </summary>
public class StyleProperty : Collection<KeyValuePair<string, object>>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">用匿名类型表示样式的值。如 <c>new { width = "100px", border = "1px solid #ccc" ... }</c></param>
    public StyleProperty([NotNull] object values) : this(values.GetType().GetProperties().Select(m => new KeyValuePair<string, object>(m.Name, m!.GetValue(values))).ToList())
    {

    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">样式的值。</param>
    public StyleProperty([NotNull] IList<KeyValuePair<string, object>> values) : base(values)
    {
    }

    /// <summary>
    /// 以样式返回CSS区域字符串。
    /// </summary>
    public override string ToString()
    => Items.Select(m => $"{m.Key}:{m.Value};").Aggregate((prev, next) => $"{prev} {next}");
}

/// <summary>
/// 表示样式中的关键帧集合。
/// </summary>
public class StyleKeyFrame : Collection<KeyValuePair<string, StyleProperty>>
{
    /// <summary>
    /// 添加一个具有指定名称和值的新关键帧。
    /// </summary>
    /// <param name="name">关键帧的名称。</param>
    /// <param name="values">关键帧的值。</param>
    public StyleKeyFrame Add(string name, object values)
    {
        Add(new KeyValuePair<string, StyleProperty>(name, new(values)));
        return this;
    }

    /// <summary>
    /// 返回 @keyframes 区域的 CSS 字符串。
    /// </summary>
    public override string ToString()
    => Items.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev} {next}");
}