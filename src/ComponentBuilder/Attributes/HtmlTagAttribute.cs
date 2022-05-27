namespace ComponentBuilder;

/// <summary>
/// 表示组件要渲染的 HTML 元素的名称。
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class HtmlTagAttribute : Attribute
{
    /// <summary>
    /// 初始化 <see cref="HtmlTagAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">HTML 元素名称。</param>
    public HtmlTagAttribute(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    /// <summary>
    /// 获取 HTML 元素名称。
    /// </summary>
    public string Name { get; }
}
