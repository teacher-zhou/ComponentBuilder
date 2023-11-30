using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;
/// <summary>
/// A selector that represents a style.
/// </summary>
public class StyleSelector
{
    readonly Dictionary<string, string> _selectors = [];

    /// <summary>
    /// A style with a specified key and style value.
    /// </summary>
    /// <param name="selector">CSS selector.</param>
    /// <param name="property">The style property value.</param>
    /// <exception cref="ArgumentException"><paramref name="selector"/> is null or empty.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="property"/> is null。</exception>
    public StyleSelector Add(string selector, StyleProperty property)
    {
        if (string.IsNullOrEmpty(selector))
        {
            throw new ArgumentException($"'{nameof(selector)}' cannot be null or empty.", nameof(selector));
        }

        if (property is null)
        {
            throw new ArgumentNullException(nameof(property));
        }
        selector = selector.Replace("_", "-");
        _selectors[selector] = property.ToString();
        return this;
    }

    /// <summary>
    /// A style with a specified key and style value.
    /// </summary>
    /// <param name="selector">CSS selector.</param>
    /// <param name="styleString">The style property string.</param>
    /// <exception cref="ArgumentException"><paramref name="selector"/> or <paramref name="styleString"/> is null or empty.</exception>
    public StyleSelector Add(string selector, string styleString)
    {
        if (string.IsNullOrEmpty(selector))
        {
            throw new ArgumentException($"'{nameof(selector)}' cannot be null or empty.", nameof(selector));
        }

        if (string.IsNullOrEmpty(styleString))
        {
            throw new ArgumentException($"'{nameof(styleString)}' cannot be null or empty.", nameof(styleString));
        }
        selector = selector.Replace("_", "-");
        _selectors[selector] = styleString;
        return this;
    }

    /// <summary>
    /// Returns a CSS region string in style.
    /// </summary>
    public override string ToString()
    => _selectors.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev}\n{next}");
}

/// <summary>
/// Represents a collection of CSS key/value pairs in a style area.
/// </summary>
/// <remarks>
/// <inheritdoc/>
/// </remarks>
/// <param name="values">The style key/value paires.</param>
public class StyleProperty([NotNull] IList<KeyValuePair<string, object?>> values) : Collection<KeyValuePair<string, object?>>(values)
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="values">The value of the style is represented by an anonymous type. E.g.  <c>new { width = "100px", border = "1px solid #ccc" ... }</c></param>
    public StyleProperty([NotNull] object values) : this(values.GetType().GetProperties().Select(m => new KeyValuePair<string, object?>(m.Name, m.GetValue(values))).ToList())
    {

    }

    /// <summary>
    /// Returns a CSS region string in style.
    /// </summary>
    public override string ToString()
    => Items.Select(m => $"{m.Key}:{m.Value};").Aggregate((prev, next) => $"{prev} {next}");
}

/// <summary>
/// Represents a collection of keyframes in a style.
/// </summary>
public class StyleKeyFrame : Collection<KeyValuePair<string, StyleProperty>>
{
    /// <summary>
    /// Adds a new keyframe with the specified name and value.
    /// </summary>
    /// <param name="name">Name of the keyframe.</param>
    /// <param name="values">Value of the keyframe.</param>
    public StyleKeyFrame Add(string name, object values)
    {
        Add(new KeyValuePair<string, StyleProperty>(name, new(values)));
        return this;
    }

    /// <summary>
    /// Returns the CSS string in the @keyframes area.
    /// </summary>
    public override string ToString()
    => Items.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev} {next}");
}