using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Represents to resolve html element tag in component.
    /// </summary>
    public class HtmlTagAttributeResolver : IComponentParameterResolver<string>
    {
        /// <summary>
        /// Resolve <see cref="HtmlTagAttribute"/> from component.
        /// </summary>
        /// <param name="component">The component to resolve.</param>
        /// <returns>A string value of <see cref="HtmlTagAttribute.Name"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        public string Resolve(ComponentBase component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            return component.GetType().GetCustomAttribute<HtmlTagAttribute>()?.Name ?? "div";
        }
    }
}
