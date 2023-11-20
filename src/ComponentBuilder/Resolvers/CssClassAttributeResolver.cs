using System.Reflection;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// 定义组件参数的 CSS class 解析器。
/// </summary>
public interface IParameterClassResolver : IComponentResolver<IEnumerable<string>>
{
}

/// <summary>
/// 识别组件参数标记了 <see cref="CssClassAttribute"/> 特性并生成 HTML class 属性的值。
/// </summary>
class CssClassAttributeResolver :IParameterClassResolver
{
    private readonly ICollection<string> _cssList;

    /// <summary>
    /// Initializes a new instance of <see cref="CssClassAttributeResolver"/> class.
    /// </summary>
    public CssClassAttributeResolver()
    {
        _cssList = new List<string>();
    }

    /// <inheritdoc/>
    public IEnumerable<string> Resolve(IBlazorComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var componentType = component.GetType();

        //support interface CssClassAttribute

        var componentInterfaceTypes = componentType.GetInterfaces();

        //interface is defined CssClassAttribute
        var interfaceCssClassAttributes = componentInterfaceTypes.Where(m => m.IsDefined(typeof(CssClassAttribute), true)).Select(m => m.GetCustomAttribute<CssClassAttribute>());

        // for component that defined CssClassAttribute, could concat value with interface has pre-definition of CssClassAttribute

        // Question:
        // How to disable to concat with interface pre-definition of CssClassAttribute?

        List<(string name, object? value, CssClassAttribute attr, bool isParameter)> stores = new();


        foreach (var item in interfaceCssClassAttributes)
        {
            if (!CanApplyCss(item!))
            {
                continue;
            }
            stores.Add(new(item!.CSS!, null, item, false));
        }

        ApplyCssFromComponentType(componentType, stores);


        //interface properties is defined CssClassAttribute
        var interfacePropertisWithCssClassAttributes = componentInterfaceTypes
            .SelectMany(m => m.GetProperties())
            .Where(m => m.IsDefined(typeof(CssClassAttribute), true));

        //class properties is defined CssClassAttribute
        var classPropertiesWithCssAttributes = componentType.GetProperties()
            .Where(m => m.IsDefined(typeof(CssClassAttribute), true));

        //override same key & value from class property
        var mergeCssClassAttributes = CompareToTake(interfacePropertisWithCssClassAttributes, classPropertiesWithCssAttributes);

        var cssClassValuePaires = GetParametersCssClassAttributes(mergeCssClassAttributes, component);

        stores.AddRange(cssClassValuePaires!);

        foreach (var parameters in stores.OrderBy(m => m.attr.Order))
        {
            var name = parameters.name; // name of CssClassAttribute or parameter name
            var value = parameters.value; //value of parameter
            var attr = parameters.attr; //CssClassAttribute

            var css = string.Empty;

            if (!parameters.isParameter) //is parameter or pre-definition
            {
                css = name;
            }
            else
            {
                switch (value)
                {
                    case null:
                        if (attr is NullCssClassAttribute nullCssClassAttribute)
                        {
                            css = nullCssClassAttribute.CSS;
                        }
                        break;
                    case bool:
                        if (attr is BooleanCssClassAttribute boolAttr)
                        {
                            css = (bool)value ? boolAttr.TrueClass : boolAttr.FalseClass;
                        }
                        else if ((bool)value)
                        {
                            css = name;
                        }
                        break;
                    case Enum://css + enum css
                        value = ((Enum)value).GetCssClass();
                        goto default;
                    case Enumeration:
                        value = ((Enumeration)value).Value;
                        goto default;
                    default:// css + value

                        name ??= string.Empty;

                        if (name.IndexOf("{0}") <= 0)
                        {
                            name = $"{name}{{0}}";
                        }

                        css = string.Format(name, value);// suffix ? $"{value}{name}" : $"{name}{value}";
                        break;
                }
            }
            _cssList.Add(css!);
        }

        return _cssList;


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

        static IEnumerable<(string name, object? value, CssClassAttribute attr, bool isParameter)> GetParametersCssClassAttributes(IEnumerable<PropertyInfo> properties, object instance)
        {
            return properties!.Where(m => m.IsDefined(typeof(CssClassAttribute), true))
                .Select(m => new { property = m, attr = m.GetCustomAttribute<CssClassAttribute>(true) })
                .Where(m => CanApplyCss(m.attr))
                .Select(m => (name: m.attr.CSS!, value: m.property.GetValue(instance), m.attr, true))
                ;
        }

        static bool CanApplyCss(CssClassAttribute attribute) => !attribute.Disabled;

        static void ApplyCssFromComponentType(Type componentType, List<(string name, object? value, CssClassAttribute? attr, bool isParameter)> stores)
        {
            if (componentType.TryGetCustomAttribute<CssClassAttribute>(out var classCssAttribute, true) && CanApplyCss(classCssAttribute!))
            {
                stores.Add(new(classCssAttribute!.CSS!, default, classCssAttribute, false));

                if (classCssAttribute.Inherited)
                {
                    ApplyCssFromComponentType(componentType.BaseType, stores);
                }
            }
        }
    }
}
