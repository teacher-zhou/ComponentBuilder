using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// A default resolver to resolve <see cref="HtmlEventAttribute"/> for parameters.
/// </summary>
public class HtmlEventAttributeResolver : IHtmlEventAttributeResolver
{
    /// <summary>
    /// Resolve parameters witch defined <see cref="HtmlEventAttribute"/> in <paramref name="component"/> component.
    /// </summary>
    /// <param name="component">The component to be resolved.</param>
    /// <returns>A key/value pair collection contains event name and callback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public IEnumerable<KeyValuePair<string, object>> Resolve(ComponentBase component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var componentType = component.GetType();

        return componentType.GetInterfaces()
            .SelectMany(m => m.GetProperties())
            .GetEventNameValue(component)
            .Merge(componentType.GetProperties().GetEventNameValue(component));
            ;
    }
}
