using OneOf;
using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// 解析定义 <see cref="CssClassAttribute"/> 的组件和参数。
/// </summary>
public class CssClassAttributeResolver : ComponentParameterResolver<string?>, ICssClassAttributeResolver
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

    /// <inheritdoc/>
    protected override string? Resolve(ComponentBase component)
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
            if (!CanApplyCss(item!, component))
            {
                continue;
            }
            stores.Add(new(item!.Name!, null, item, false));
        }

        if (componentType.TryGetCustomAttribute<CssClassAttribute>(out var classCssAttribute, true) && CanApplyCss(classCssAttribute!, component))
        {
            stores.Add(new(classCssAttribute!.Name!, null, classCssAttribute, false));
        }


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
            var name = parameters.name; //CssClassAttribute 的 Name 或 参数的属性名
            var value = parameters.value; //参数的值
            var attr = parameters.attr; //CssClassAttribute 的特性

            var css = string.Empty;

            if (!parameters.isParameter) //判断是否是参数，因为类或接口也可以设置 CssClassAttribute
            {
                css = name;
            }
            else
            {
                if (value is IOneOf oneOf)
                {
                    value = oneOf.Value;
                }

                switch (value)
                {
                    case null:
                        if (attr is NullCssClassAttribute nullCssClassAttribute)
                        {
                            css = nullCssClassAttribute.Name;
                        }
                        break;
                    case bool:
                        if (attr is BooleanCssClassAttribute boolAttr)
                        {
                            css = (bool)value ? boolAttr.TrueCssClass : boolAttr.FalseCssClass;
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

                        css = String.Format(name, value);// suffix ? $"{value}{name}" : $"{name}{value}";
                        break;
                }
            }
            _cssClassBuilder.Append(css!);
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
        static IEnumerable<(string name, object? value, CssClassAttribute attr, bool isParameter)> GetParametersCssClassAttributes(IEnumerable<PropertyInfo> properties, object instance)
        {
            return properties!.Where(m => m.IsDefined(typeof(CssClassAttribute), true))
                .Select(m => new { property = m, attr = m.GetCustomAttribute<CssClassAttribute>(true) })
                .Where(m => CanApplyCss(m.attr, m.property.GetValue(instance)))
                .Select(m => (name: m.attr.Name!, value: m.property.GetValue(instance), m.attr, true))
                ;
        }

        static bool CanApplyCss(CssClassAttribute attribute, object? value) => !attribute.Disabled;
    }
}
