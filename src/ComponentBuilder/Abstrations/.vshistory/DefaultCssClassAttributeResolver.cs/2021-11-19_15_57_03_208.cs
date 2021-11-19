using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    public class DefaultCssClassAttributeResolver : ICssClassResolver
    {

        public DefaultCssClassAttributeResolver(ICssClassBuilder cssClassBuilder)
        {
        }

        public virtual string Resolve(object component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }
            var classList = new List<string>();

            var componentType = component.GetType();
            if (componentType.TryGetAttribute<CssClassAttribute>(out var classCssAttribute))
            {
                classList.Add(classCssAttribute.Css);
            }

            foreach (var parameters in componentType.GetProperties().Where(m => m.IsDefined(typeof(CssClassAttribute))))
            {
                var name = parameters.Name;
                var value = parameters.GetValue(component);

                var css = parameters.GetCustomAttribute<CssClassAttribute>().Css;



                if (value is null)
                {
                    continue;
                }

                switch (value)
                {
                    case Boolean:
                        if ((bool)value)
                        {
                            classList.Add($"{css}");
                        }
                        break;
                    case Enum://css + enum css
                        classList.Add($"{css}{((Enum)value).GetCssClass()}");
                        break;
                    default:// css + value
                        classList.Add($"{css}{value}");
                        break;
                }
            }

            return string.Join(" ", classList);
        }
    }
}
