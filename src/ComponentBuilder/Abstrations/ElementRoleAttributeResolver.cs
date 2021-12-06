using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Represents to resolve html element role attribute in component.
    /// </summary>
    public class ElementRoleAttributeResolver : IComponentParameterResolver<string>
    {
        /// <summary>
        /// Resolve <see cref="ElementRoleAttribute"/> from component.
        /// </summary>
        /// <param name="component">The component to resolve.</param>
        /// <returns>A string value of <see cref="ElementRoleAttribute.Name"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        public string Resolve(BlazorComponentBase component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            return component.GetType().GetCustomAttribute<ElementRoleAttribute>()?.Name;
        }
    }
}
