using System.Reflection;

namespace ComponentBuilder.Automation.Resolvers;

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

        var type = component.GetType();

        HtmlTagAttribute? attribute = type.GetCustomAttribute<HtmlTagAttribute>();

        attribute??= type.GetInterfaces().SingleOrDefault(m => m.IsDefined(typeof(HtmlTagAttribute)))?.GetCustomAttribute<HtmlTagAttribute>();

        return attribute?.Name ?? "div";
    }
}
