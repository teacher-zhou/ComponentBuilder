using System.Linq;

namespace ComponentBuilder;

/// <summary>
/// The helper for html elements.
/// </summary>
public static class HtmlHelper
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
}
