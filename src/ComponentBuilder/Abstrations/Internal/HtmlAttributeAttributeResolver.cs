using System.Collections.Generic;
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

        var parameterAttributes = GetHtmlAttributes<HtmlAttributeAttribute>(component);

        return attributes.Merge(parameterAttributes);

        //static string GetHtmlAttributeValue(PropertyInfo property, object? value)
        //=> value switch
        //{
        //    bool b => b ? property.Name.ToLower() : default,
        //    Enum => ((Enum)value).GetHtmlAttribute(),
        //    _ => value?.ToString()?.ToLower(),
        //} ?? string.Empty;
    }

    protected IEnumerable<KeyValuePair<string,object?>> GetHtmlAttributes<THtmlAttribute>(ComponentBase component) where THtmlAttribute:HtmlAttributeAttribute
    {
        var parameterAttributes = new Dictionary<string, object>();

        component.GetType()
             .GetProperties()
             .Where(m => m.IsDefined(typeof(THtmlAttribute)))
             .ToList().ForEach(property =>
             {

                 var attr = property.GetCustomAttribute<HtmlAttributeAttribute>();
                 if ( attr is null )
                 {
                     return;
                 }
                 var propertyValue = property.GetValue(component);

                 var name = attr?.Name ?? property.Name.ToLower();
                 
                 if ( propertyValue is not null )
                 {
                     if ( propertyValue is bool boolValue )
                     {
                         if ( boolValue )
                         {
                             parameterAttributes.Add(name, attr?.Value?? name);
                         }
                     }
                     else if ( propertyValue is Enum @enum )
                     {
                         parameterAttributes.Add(name, @enum.GetHtmlAttribute());
                     }
                     else
                     {
                         parameterAttributes.Add(name, attr?.Value?? propertyValue);
                     }
                 }
             });
        return parameterAttributes;
    }
}
