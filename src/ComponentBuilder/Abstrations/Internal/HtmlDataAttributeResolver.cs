using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// Represents to resolve element property from parameters.
/// </summary>
public class HtmlDataAttributeResolver : IHtmlAttributesResolver
{
    /// <summary>
    /// Resolve <see cref="HtmlDataAttribute"/> from parameters in component.
    /// </summary>
    /// <param name="component">The component to resolve.</param>
    /// <returns>A key value pair collection present by name and value of properties.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public IEnumerable<KeyValuePair<string, object>> Resolve(ComponentBase component)
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
                new KeyValuePair<string, object>(property.GetCustomAttribute<HtmlDataAttribute>()?.Name ?? $"data-{property.Name.ToLower()}" ,
                                                property.GetCustomAttribute<HtmlDataAttribute>()?.Value ?? GetHtmlAttributeValue(property, property.GetValue(component)))
                                                );
        return attributes.Merge(parameterAttributes);

        object GetHtmlAttributeValue(PropertyInfo property, object value)
        => value switch
        {
            bool => property.Name.ToLower(),
            Enum => ((Enum)value).GetHtmlAttribute(),
            _ => value?.ToString()?.ToLower(),
        };
    }
}
