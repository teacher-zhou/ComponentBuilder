using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Represents to resolve html element tag in component.
    /// </summary>
    public class ElementTagAttributeResolver : IComponentParameterResolver<string>
    {
        /// <summary>
        /// Resolve <see cref="ElementTagAttribute"/> from component.
        /// </summary>
        /// <param name="component">The component to resolve.</param>
        /// <returns>A string value of <see cref="ElementTagAttribute.Name"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        public string Resolve(ComponentBase component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            return component.GetType().GetCustomAttribute<ElementTagAttribute>()?.Name ?? "div";
        }
    }
}
