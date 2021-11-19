using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Abstrations
{
    internal class CssClassAttributeResolver : ICssClassResolver
    {
        private readonly ICssClassBuilder _cssClassBuilder;

        public CssClassAttributeResolver(ICssClassBuilder cssClassBuilder)
        {
            this._cssClassBuilder = cssClassBuilder;
        }

        public void Resolve(Type componentType)
        {
            if (componentType.TryGetAttribute<CssClassAttribute>(out var classCssAttribute))
            {
                _cssClassBuilder.Append(classCssAttribute.Css);
            }

            foreach (var parameters in componentType.GetProperties().Where(m => m.IsDefined(typeof(CssClassAttribute))))
            {
                var name = parameters.Name;
                var value = parameters.GetValue(componentType);
            }
        }
    }
}
