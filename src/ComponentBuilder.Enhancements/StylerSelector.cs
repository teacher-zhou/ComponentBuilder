using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;
/// <summary>
/// Represents a select of style.
/// </summary>
public class StyleSelector
{
    readonly Dictionary<string, string> _selectors = new();


    /// <summary>
    /// A style with specified key and style values.
    /// </summary>
    /// <param name="key">The key of CSS selector.</param>
    /// <param name="value">The value of key.</param>
    /// <exception cref="ArgumentException"><paramref name="key"/> is null or empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null。</exception>
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
    /// A style with specified key and style values.
    /// </summary>
    /// <param name="key">The key of CSS selector.</param>
    /// <param name="styleString">The value of key.</param>
    /// <exception cref="ArgumentException"><paramref name="key"/> or <paramref name="styleString"/> is null or empty.</exception>
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
    /// Returns the CSS region string in style.
    /// </summary>
    public override string ToString()
    => _selectors.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev}\n{next}");
}

/// <summary>
/// Represents a collection for CSS key/value pairs in style region.
/// </summary>
public class StyleProperty : Collection<KeyValuePair<string, object>>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">A value of styles.</param>
    public StyleProperty([NotNull] object values) : this(values.GetType().GetProperties().Select(m => new KeyValuePair<string, object>(m.Name, m!.GetValue(values))).ToList())
    {

    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">A list for key/value pairs of style.</param>
    public StyleProperty([NotNull] IList<KeyValuePair<string, object>> values) : base(values)
    {
    }

    /// <summary>
    /// Returns the CSS region string in style.
    /// </summary>
    public override string ToString()
    => Items.Select(m => $"{m.Key}:{m.Value};").Aggregate((prev, next) => $"{prev} {next}");
}

/// <summary>
/// Represents a collection of keyframe in style.
/// </summary>
public class StyleKeyFrame : Collection<KeyValuePair<string, StyleProperty>>
{
    /// <summary>
    /// Add a new keyframe with specified name and values.
    /// </summary>
    /// <param name="name">The name of keyframe.</param>
    /// <param name="values">The value of name.</param>
    public StyleKeyFrame Add(string name, object values)
    {
        Add(new KeyValuePair<string, StyleProperty>(name, new(values)));
        return this;
    }

    /// <summary>
    /// Returns the value for @keyframes region.
    /// </summary>
    public override string ToString()
    => Items.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev} {next}");
}