namespace ComponentBuilder;

/// <summary>
/// 表示组件类或参数将生成 HTML 属性。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false)]
public class HtmlAttributeAttribute : Attribute
{
    /// <summary>
    /// 初始化 <see cref="HtmlAttributeAttribute"/> 类的新实例。
    /// </summary>
    public HtmlAttributeAttribute() : this(null)
    {

    }
    /// <summary>
    /// 使用指定名称初始化 <see cref="HtmlAttributeAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">HTML 元素的属性名称。</param>
    public HtmlAttributeAttribute(string? name)
    {
        Name = name;
    }

    /// <summary>
    /// 获取属性的名称。
    /// </summary>
    public string? Name { get; }
    /// <summary>
    /// 获取或设置固定的属性值。
    /// </summary>
    public object? Value { get; set; }
}
