using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations;

/// <summary>
/// Represents to resolve element property from parameters.
/// </summary>
public class HtmlAttributeAttributeResolver : IHtmlAttributesResolver
{
    /// <summary>
    /// Resolve <see cref="HtmlAttributeAttribute"/> from parameters in component.
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

        if (componentType.TryGetCustomAttribute(out HtmlAttributeAttribute attribute))
        {
            attributes.Add(attribute.Name, attribute.Value);
        }
        var parameterAttributes = componentType
            .GetProperties()
            .Where(m => m.IsDefined(typeof(HtmlAttributeAttribute)))
            .Select(
                property =>
                new KeyValuePair<string, object>(property.GetCustomAttribute<HtmlAttributeAttribute>()?.Name ?? property.Name.ToLower(),
                                                property.GetCustomAttribute<HtmlAttributeAttribute>()?.Value ?? property.GetValue(component))
                                                );
        return attributes.Merge(parameterAttributes);

    }
}
