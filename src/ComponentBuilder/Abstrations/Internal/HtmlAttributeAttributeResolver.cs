using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// <inheritdoc/>
/// </summary>
internal class HtmlAttributeAttributeResolver : ComponentParameterResolverBase<IEnumerable<KeyValuePair<string, object>>>, IHtmlAttributesResolver
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
            .SkipWhile(property => property.GetValue(component) is bool boolValue && !boolValue)
            .Select(
                property =>
                new KeyValuePair<string, object>(
                    property.GetCustomAttribute<HtmlAttributeAttribute>()?.Name ?? property.Name.ToLower(),
                    property.GetCustomAttribute<HtmlAttributeAttribute>()?.Value ?? GetHtmlAttributeValue(property, property.GetValue(component))
                    )
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
