using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// Resolve <see cref="HtmlDataAttribute"/> from parameter.
/// </summary>
internal class HtmlDataAttributeResolver : HtmlAttributeResolverBase
{
    /// <inheritdoc/>
    protected override IEnumerable<KeyValuePair<string, object>> Resolve(ComponentBase component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var componentType = component.GetType();

        var attributes = new Dictionary<string, object>();
        var parameterAttributes = componentType
            .GetProperties()
            .Where(m => m.IsDefined(typeof(HtmlDataAttribute)))
            .Select(
                property =>
                new KeyValuePair<string, object>(property.GetCustomAttribute<HtmlDataAttribute>()?.Name ?? $"data-{property.Name.ToLower()}",
                                                property.GetCustomAttribute<HtmlDataAttribute>()?.Value ?? GetHtmlAttributeValue(property, property.GetValue(component)!))
                                                );
        return attributes.Merge(parameterAttributes);

        static object GetHtmlAttributeValue(PropertyInfo property, object value)
        {
            string? v = value switch
            {
                bool => property.Name.ToLower(),
                Enum => ((Enum)value).GetHtmlAttribute(),
                _ => value?.ToString()?.ToLower(),
            };
            return v!;
        }
    }
}
