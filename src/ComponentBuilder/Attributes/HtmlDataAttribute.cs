using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// 应用组件的参数。作为 HTML 的 data 属性，并与指定字符串拼接作为 HTML 的完整属性。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
public class HtmlDataAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 初始化 of <see cref="HtmlDataAttribute"/> 类的新实例。
    /// </summary>
    public HtmlDataAttribute()
    {

    }

    /// <summary>
    /// 使用指定名称初始化 <see cref="HtmlDataAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">指定的 data 名称，将拼接成 <c>data-{name}</c> 字符串。</param>
    public HtmlDataAttribute([NotNull] string name) : base($"data-{name}")
    {
    }
}
