using System.Reflection;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// Defines a resolver for <see cref="HtmlTagAttribute"/> to recognize HTML tag name for creating element.
/// </summary>
public interface IHtmlTagAttributeResolver : IComponentParameterResolver<string>
{

}

/// <summary>
/// Resolve <see cref="HtmlTagAttribute"/> form component.
/// </summary>
class HtmlTagAttributeResolver : IHtmlTagAttributeResolver
{
    /// <inheritdoc/>
    public string Resolve(IBlazorComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        var type = component.GetType();

        HtmlTagAttribute? attribute = type.GetCustomAttribute<HtmlTagAttribute>();

        attribute??= type.GetInterfaces().SingleOrDefault(m => m.IsDefined(typeof(HtmlTagAttribute)))?.GetCustomAttribute<HtmlTagAttribute>();

        return attribute?.Name ?? "div";
    }
}
