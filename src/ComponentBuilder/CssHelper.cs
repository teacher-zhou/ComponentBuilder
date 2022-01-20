using System.Linq;
using System.Text;

using ComponentBuilder.Abstrations.Internal;

namespace ComponentBuilder;

/// <summary>
/// The helper for css.
/// </summary>
public static class CssHelper
{
    /// <summary>
    /// Merge html attribtes.
    /// </summary>
    /// <param name="htmlAttributes">The html attributes.</param>
    /// <returns>A key/value pairs of attributes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="htmlAttributes"/> is <c>null</c>.</exception>
    public static IEnumerable<KeyValuePair<string, object>> MergeAttributes(object htmlAttributes)
    {
        if (htmlAttributes is null)
        {
            throw new ArgumentNullException(nameof(htmlAttributes));
        }

        if (htmlAttributes is IEnumerable<KeyValuePair<string, object>> keyValueAttributes)
        {
            return keyValueAttributes;
        }

        return htmlAttributes.GetType().GetProperties()
            .Select(property =>
            {
                var name = property.Name.Replace("_", "-");
                var value = property.GetValue(htmlAttributes);
                return new KeyValuePair<string, object>(name, value);
            }).Distinct();
    }

    /// <summary>
    /// Returns css class that is satisfied by condition.
    /// </summary>
    /// <param name="condition">A condition that is satisfied.</param>
    /// <param name="trueValue">A string value weither <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="falseValue">A string value weither <paramref name="condition"/> is <c>false</c>.</param>
    /// <param name="prepend">A fixed string to prepend and has a space seperator behind if value not null or empty.</param>
    /// <returns>A css class string.</returns>
    public static string GetCssClass(bool condition, string trueValue, string? falseValue = default, string? prepend = default)
    {
        var builder = new StringBuilder();
        if (!string.IsNullOrEmpty(prepend))
        {
            builder.Append(prepend).Append(' ');
        }
        return builder.Append(condition ? trueValue : falseValue).ToString();
    }

    /// <summary>
    /// Create CSS class builder.
    /// </summary>
    /// <returns>A new instance of <see cref="DefaultCssClassBuilder"/> class.</returns>
    public static ICssClassBuilder CreateBuilder() => new DefaultCssClassBuilder();
}
