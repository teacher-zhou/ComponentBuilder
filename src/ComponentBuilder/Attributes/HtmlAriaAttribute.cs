using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// 支持 aria-* 的 HTML 属性。
/// </summary>
public class HtmlAriaAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 初始化 <see cref="HtmlAriaAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">Aria 的名称，即 aria-{name} 的值。</param>
    public HtmlAriaAttribute([NotNull] string name) : base($"aria-{name}")
    {
    }
}
