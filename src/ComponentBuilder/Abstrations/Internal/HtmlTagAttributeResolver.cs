using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// 解析 <see cref="HtmlTagAttribute"/> 解析器。
/// </summary>
public class HtmlTagAttributeResolver : ComponentParameterResolver<string>, IComponentParameterResolver<string>
{
    /// <inheritdoc/>
    protected override string Resolve(ComponentBase component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        return component.GetType().GetCustomAttribute<HtmlTagAttribute>()?.Name ?? "div";
    }
}
