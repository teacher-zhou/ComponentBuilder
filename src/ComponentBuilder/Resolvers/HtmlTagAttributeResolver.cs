using System.Reflection;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// Resolve <see cref="HtmlTagAttribute"/> form component.
/// </summary>
public class HtmlTagAttributeResolver : IComponentParameterResolver<string>
{
    /// <inheritdoc/>
    public string Resolve(IBlazorComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        return component.GetType().GetCustomAttribute<HtmlTagAttribute>()?.Name ?? "div";
    }
}
