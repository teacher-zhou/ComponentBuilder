using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// Resolve <see cref="HtmlAttributeAttribute"/> from parameter.
/// </summary>
internal class HtmlAttributeAttributeResolver : HtmlAttributeResolverBase
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

        if (componentType.TryGetCustomAttribute(out HtmlAttributeAttribute? attribute))
        {
            attributes.Add(attribute!.Name!, attribute.Value ?? string.Empty);
        }
        var parameterAttributes = componentType
            .GetProperties()
            .Where(m => m.IsDefined(typeof(HtmlAttributeAttribute)))
            .Where(property => property.GetValue(component) is bool boolValue && boolValue)
            .Select(property =>
                    {
                        var attr = property.GetCustomAttribute<HtmlAttributeAttribute>();

                        return new KeyValuePair<string, object>(
                            attr?.Name ?? property.Name.ToLower(),
                            attr?.Value ?? GetHtmlAttributeValue(property, property.GetValue(component))
                        );
                    }
            );
        return attributes.Merge(parameterAttributes);

        static string GetHtmlAttributeValue(PropertyInfo property, object? value)
        => value switch
        {
            bool b => b ? property.Name.ToLower() : default,
            Enum => ((Enum)value).GetHtmlAttribute(),
            _ => value?.ToString()?.ToLower(),
        } ?? string.Empty;
    }
}
