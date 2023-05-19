using System.Reflection;

namespace ComponentBuilder.Resolvers;

/// <summary>
/// 提供对 <see cref="HtmlTagAttribute"/> 特性的识别。
/// </summary>
public interface IHtmlTagAttributeResolver : IComponentResolver<string>
{

}

/// <summary>
/// 解析组件标记了 <see cref="HtmlTagAttribute"/> 特性并创建相应的 HTML 元素名称。
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
