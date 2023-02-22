using System.Collections.Generic;
using System.Reflection;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// Resolve <see cref="HtmlAttributeAttribute"/> from parameter.
/// </summary>
public class HtmlAttributeAttributeResolver : IHtmlAttributeResolver
{
    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<string, object>> Resolve(IBlazorComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var componentType = component.GetType();

        var attributes = new Dictionary<string, object>();

        //Get interfaces witch define HtmlAttribute
        attributes.AddOrUpdateRange(componentType.GetInterfaces().Where(m => m.IsDefined(typeof(HtmlAttributeAttribute)))
            .SelectMany(m => m.GetCustomAttributes<HtmlAttributeAttribute>())
            .Select(m => new KeyValuePair<string, object>(m.Name!, m.Value ?? string.Empty)));

        // Get class defined HtmlAttribute
        componentType.GetCustomAttributes<HtmlAttributeAttribute>().ForEach(attribute =>
        {
            attributes.AddOrUpdate(new(attribute!.Name!, attribute.Value ?? string.Empty));
        });

        //Get interfaces properties whitch defined HtmlAttribute

        componentType.GetInterfaces().ForEach(type =>
        {
           attributes.AddOrUpdateRange(GetHtmlAttributes<HtmlAttributeAttribute>(component, type));
        });

        var parameterAttributes = GetHtmlAttributes<HtmlAttributeAttribute>(component);

        attributes.AddOrUpdateRange(parameterAttributes);

        return attributes;
    }

    protected IEnumerable<KeyValuePair<string, object>> GetHtmlAttributes<THtmlAttribute>(object instance,Type? type=default) where THtmlAttribute : HtmlAttributeAttribute
    {
        var parameterAttributes = new Dictionary<string, object>();

        (type?? instance.GetType())
             .GetProperties()
             .Where(m => m.IsDefined(typeof(THtmlAttribute)))
             .ForEach(property =>
             {
                 var attr = property.GetCustomAttribute<HtmlAttributeAttribute>();
                 if (attr is null)
                 {
                     return;
                 }

                 var propertyValue = property.GetValue(instance);

                 var name = attr?.Name ?? property.Name.ToLower();

                 if ( propertyValue is not null)
                 {
                     if ( propertyValue is bool boolValue )
                     {
                         if ( boolValue )
                         {
                             parameterAttributes.Add(name, attr?.Value ?? name);
                         }
                     }
                     else if ( propertyValue is Enum @enum )
                     {
                         parameterAttributes.Add(name, @enum.GetHtmlAttribute());
                     }
                     else
                     {
                         parameterAttributes.Add(name, attr?.Value ?? propertyValue);
                     }
                 }
             });
        return parameterAttributes;
    }
}
