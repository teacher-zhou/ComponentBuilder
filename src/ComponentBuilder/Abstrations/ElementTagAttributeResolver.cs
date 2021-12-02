using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    public class ElementTagAttributeResolver : IAttributeResolver<string>
    {
        public string Resolve(BlazorComponentBase component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            return component.GetType().GetCustomAttribute<ElementTagAttribute>()?.Name ?? "div";
        }
    }
}
