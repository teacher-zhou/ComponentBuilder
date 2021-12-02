using System;
using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// To resolve css class attribute for <see cref="IAttributeResolver{TResult}"/>.
    /// </summary>
    public class CssClassAttributeResolver : IAttributeResolver<string>
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
        public virtual string Resolve(BlazorComponentBase component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var componentType = component.GetType();
            if (componentType.TryGetCustomAttribute<CssClassAttribute>(out var classCssAttribute))
            {
                _cssClassBuilder.Append(classCssAttribute.Css);
            }

            foreach (var parameters in componentType.GetProperties().Where(m => m.IsDefined(typeof(CssClassAttribute))))
            {
                var name = parameters.Name;
                var value = parameters.GetValue(component);

                var css = parameters.GetCustomAttribute<CssClassAttribute>()?.Css;



                if (value is null)
                {
                    continue;
                }

                switch (value)
                {
                    case Boolean:
                        if ((bool)value)
                        {
                            _cssClassBuilder.Append($"{css}");
                        }
                        break;
                    case Enum://css + enum css
                        _cssClassBuilder.Append($"{css}{((Enum)value).GetCssClass()}");
                        break;
                    default:// css + value
                        _cssClassBuilder.Append($"{css}{value}");
                        break;
                }
            }

            return _cssClassBuilder.Build(true);
        }
    }
}
