namespace ComponentBuilder;

/// <summary>
/// 提供 Blazor 组件的功能。
/// </summary>
public interface IBlazorComponent : IComponent
{
    /// <summary>
    /// 返回元素中CSS属性的字符串。
    /// </summary>
    /// <returns>每个条目用空格隔开的字符串。</returns>
    string? GetCssClassString();
    /// <summary>
    /// 返回元素中样式属性的字符串。
    /// </summary>
    /// <returns>每一项都用';'分隔的字符串。</returns>
    string? GetStyleString();
}
