﻿using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// 解析定义 <see cref="HtmlEventAttribute"/> 的解析器。
/// </summary>
public class HtmlEventAttributeResolver : IHtmlEventAttributeResolver
{
    /// <inheritdoc/>
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
