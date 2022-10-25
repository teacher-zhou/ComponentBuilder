using System.Reflection;

namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// Resolve <see cref="HtmlTagAttribute"/> form component.
/// </summary>
internal class HtmlTagAttributeResolver : ComponentParameterResolverBase<string>, IComponentParameterResolver<string>
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
