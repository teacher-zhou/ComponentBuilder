using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations;

/// <summary>
/// To resolve css class attribute for <see cref="IComponentParameterResolver{TResult}"/>.
/// </summary>
public class CssClassAttributeResolver : ICssClassAttributeResolver
{
    private readonly ICssClassBuilder _cssClassBuilder;

    /// <summary>
    /// Initializes a new instance of <see cref="CssClassAttributeResolver"/> class.
    /// </summary>
    /// <param name="cssClassBuilder"></param>
    public CssClassAttributeResolver(ICssClassBuilder cssClassBuilder)
    {
        this._cssClassBuilder = cssClassBuilder;
    }

    /// <summary>
    /// Resolve specified component that defined <see cref="CssClassAttribute"/> attribute.
    /// </summary>
    /// <param name="component">The component to be resolved.</param>
    /// <returns>Resolved css class string seperated by space for each item.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    public virtual string Resolve(ComponentBase component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var componentType = component.GetType();

        //support interface CssClassAttribute

        var componentInterfaceTypes = componentType.GetInterfaces();

        //interface is defined CssClassAttribute
        var interfaceCssClassAttribute = componentInterfaceTypes.Where(m => m.IsDefined(typeof(CssClassAttribute))).Select(m => m.GetCustomAttribute<CssClassAttribute>()).SingleOrDefault();

        // use class CssClassAttribute first
        if (componentType.TryGetCustomAttribute<CssClassAttribute>(out var classCssAttribute))
        {
            if (!classCssAttribute.Disabled)
            {
                _cssClassBuilder.Append(classCssAttribute.Css);
            }
        }
        else if (interfaceCssClassAttribute != null)
        {
            _cssClassBuilder.Append(interfaceCssClassAttribute.Css);
        }


        //interface properties is defined CssClassAttribute
        var interfacePropertisWithCssClassAttributes = componentInterfaceTypes
            .SelectMany(m => m.GetProperties())
            .Where(m => m.IsDefined(typeof(CssClassAttribute)));

        //class properties is defined CssClassAttribute
        var classPropertiesWithCssAttributes = componentType.GetProperties()
            .Where(m => m.IsDefined(typeof(CssClassAttribute)));

        //override same key & value from class property
        var mergeCssClassAttributes = CompareToTake(interfacePropertisWithCssClassAttributes, classPropertiesWithCssAttributes);

        var cssClassValuePaires = GetCssClassAttributesInOrderFromParameters(mergeCssClassAttributes, component);

        foreach (var parameters in cssClassValuePaires)
        {
            var name = parameters.Key;
            var value = parameters.Value;

            if (value is null)
            {
                continue;
            }

            switch (value)
            {
                case Boolean:
                    if ((bool)value)
                    {
                        _cssClassBuilder.Append($"{name}");
                    }
                    break;
                case Enum://css + enum css
                    _cssClassBuilder.Append($"{name}{((Enum)value).GetCssClass()}");
                    break;
                default:// css + value
                    _cssClassBuilder.Append($"{name}{value}");
                    break;
            }
        }

        return _cssClassBuilder.Build(true);


        static IEnumerable<PropertyInfo> CompareToTake(IEnumerable<PropertyInfo> interfaces, IEnumerable<PropertyInfo> classes)
        {
            var list = interfaces.ToList();

            foreach (var item in classes)
            {
                var index = list.FindIndex(m => m.Name == item.Name);
                if (index >= 0)
                {
                    list[index] = item;
                }
                else
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// Gets CSS class value from parameters which has defined <see cref="CssClassAttribute"/> attribute.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="instace">Object to get value from property</param>
        /// <returns>A key/value pairs contains CSS class and value.</returns>
        static IEnumerable<KeyValuePair<string, object>> GetCssClassAttributesInOrderFromParameters(IEnumerable<PropertyInfo> properties, object instace)
        {
            return properties.Where(m => m.IsDefined(typeof(CssClassAttribute)))
                .Select(m => new { property = m, attr = m.GetCustomAttribute<CssClassAttribute>() })
                .Where(m=>!m.attr.Disabled)
                .OrderBy(m => m.attr.Order)
                .Select(m => new KeyValuePair<string, object>(m.attr.Css ?? m.property.Name.ToLower(), m.property.GetValue(instace)));
        }
    }
}
