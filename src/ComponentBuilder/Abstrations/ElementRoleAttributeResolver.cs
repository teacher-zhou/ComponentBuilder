using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    public class ElementRoleAttributeResolver : IAttributeResolver<string>
    {
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
