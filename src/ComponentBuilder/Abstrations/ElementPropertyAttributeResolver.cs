using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations;

/// <summary>
/// Represents to resolve element property from parameters.
/// </summary>
public class ElementPropertyAttributeResolver : IElementAttributesResolver
{
    /// <summary>
    /// Resolve <see cref="ElementAttributeAttribute"/> from parameters in component.
    /// </summary>
    /// <param name="component">The component to resolve.</param>
    /// <returns>A key value pair collection present by name and value of properties.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public IEnumerable<KeyValuePair<string, object>> Resolve(BlazorComponentBase component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var componentType = component.GetType();
        return
            componentType
            .GetProperties()
            .Where(m => m.IsDefined(typeof(ElementAttributeAttribute)))
            .Select(
                property =>
                new KeyValuePair<string, object>(property.GetCustomAttribute<ElementAttributeAttribute>()?.Name,
                                                 property.GetValue(component))
                                                )
                    ;

    }
}
