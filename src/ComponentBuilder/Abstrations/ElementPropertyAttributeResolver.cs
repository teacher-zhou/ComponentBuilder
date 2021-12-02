using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations;

public class ElementPropertyAttributeResolver : IAttributeResolver<IEnumerable<KeyValuePair<string, object>>>
{
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
            .Where(m => m.IsDefined(typeof(ElementPropertyAttribute)))
            .Select(
                property =>
                new KeyValuePair<string, object>(property.GetCustomAttribute<ElementPropertyAttribute>()?.Name,
                                                 property.GetValue(component))
                                                )
                    ;

    }
}
