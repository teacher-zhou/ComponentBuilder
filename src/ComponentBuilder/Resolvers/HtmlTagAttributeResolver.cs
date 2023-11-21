using System.Reflection;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// Provides recognition of the <see cref="HtmlTagAttribute"/> attribute.
/// </summary>
public interface IHtmlTagAttributeResolver : IComponentResolver<string>
{

}

/// <summary>
/// The parsing component tags the <see cref="HtmlTagAttribute"/> attribute and creates the corresponding HTML element name.
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
