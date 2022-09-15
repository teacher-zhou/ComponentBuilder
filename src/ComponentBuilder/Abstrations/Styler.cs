using System.Collections.ObjectModel;

namespace ComponentBuilder;
/// <summary>
/// 表示样式的选择器。
/// </summary>
public class StyleSelector
{
    readonly Dictionary<string, string> selectors = new();


    /// <summary>
    /// 添加指定关键字的样式。
    /// </summary>
    /// <param name="key">指定关键字。</param>
    /// <param name="properties">CSS 样式。</param>
    /// <exception cref="ArgumentException"><paramref name="key"/> 不是空。</exception>
    /// <exception cref="ArgumentNullException"><paramref name="properties"/> 不能是 null。</exception>
    public StyleSelector Add(string key, StyleProperty properties)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }

        if (properties is null)
        {
            throw new ArgumentNullException(nameof(properties));
        }

        selectors[key] = properties.ToString();
        return this;
    }

    /// <summary>
    /// 添加指定关键字的样式。
    /// </summary>
    /// <param name="key">指定关键字。</param>
    /// <param name="styleString">样式字符串。</param>
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
        selectors[key] = styleString;
        return this;
    }

    /// <summary>
    /// 转换成 style 样式的字符串。
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    => selectors.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev}\n{next}");
}

/// <summary>
/// 表示具备样式属性的值。
/// </summary>
public class StyleProperty : Collection<KeyValuePair<string, object>>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">匿名类型。</param>
    public StyleProperty(object values) : this(values.GetType().GetProperties().Select(m => new KeyValuePair<string, object>(m.Name, m!.GetValue(values))).ToList())
    {

    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">属性的键值对列表。</param>
    public StyleProperty(IList<KeyValuePair<string, object>> values) : base(values)
    {
    }

    /// <summary>
    /// 转换成 style 样式的属性值的字符串。
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    => this.Items.Select(m => $"{m.Key}:{m.Value};").Aggregate((prev, next) => $"{prev} {next}");
}

/// <summary>
/// 表示样式中的关键帧。
/// </summary>
public class StyleKeyFrame : Collection<KeyValuePair<string, StyleProperty>>
{
    /// <summary>
    /// 初始化 <see cref="StyleKeyFrame"/> 类的新实例。
    /// </summary>
    /// <param name="name">关键字。</param>
    /// <param name="values">属性值。</param>
    /// <returns></returns>
    public StyleKeyFrame Add(string name, object values)
    {
        base.Add(new KeyValuePair<string, StyleProperty>(name, new(values)));
        return this;
    }

    /// <summary>
    /// 转换成 @keyframes 样式的属性值的字符串。
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    => this.Items.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev} {next}");
}