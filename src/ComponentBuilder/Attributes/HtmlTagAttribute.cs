using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// 定义组件类要生成的 HTML 元素标记的名称。
/// </summary>
[AttributeUsage(AttributeTargets.Class|AttributeTargets.Interface,AllowMultiple =false)]
public class HtmlTagAttribute : Attribute
{
    /// <summary>
    /// 使用指定的元素名称初始化 <see cref="HtmlTagAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">HTML 元素名称。</param>
    public HtmlTagAttribute([NotNull] string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

    /// <summary>
    /// 获取 HTML 元素名称。
    /// </summary>
    public string Name { get; }
}
