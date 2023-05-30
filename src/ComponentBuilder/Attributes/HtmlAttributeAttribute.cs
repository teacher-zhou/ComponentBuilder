using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// 用于生成 HTML 元素的属性。可使用在组件类、参数、接口、枚举以及字段上。
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false,Inherited =true)]
public class HtmlAttributeAttribute : Attribute
{
    /// <summary>
    /// 使用组件参数的名称作为 HTML 属性的名称以初始化 <see cref="HtmlAttributeAttribute"/> 类的新实例。
    /// </summary>
    public HtmlAttributeAttribute() : this(null)
    {

    }

    /// <summary>
    /// 使用指定的名称初始化 <see cref="HtmlAttributeAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">HTML 属性的名称。</param>
    public HtmlAttributeAttribute(string? name) => Name = name;

    /// <summary>
    /// 获取名称。
    /// </summary>
    public string? Name { get; }
    /// <summary>
    /// 获取或设置 HTML 属性的固定值。
    /// <para>
    /// 如果不设置，则使用参数的值作为 HTML 属性的值。
    /// </para>
    /// </summary>
    public string? Value { get; set; }
}


/// <summary>
/// 用于生成 "aria-" 的 HTML 特性。
/// </summary>
public class HtmlAriaAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 使用指定的名称初始化 <see cref="HtmlAriaAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">最后组合成 <c>aria-{name}</c> 格式的名称。</param>
    public HtmlAriaAttribute([NotNull] string name) : base($"aria-{name}")
    {
    }
}

/// <summary>
/// 用于生成 "data-" 的 HTML 特性。
/// </summary>
public class HtmlDataAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 使用指定的名称初始化 <see cref="HtmlDataAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">最后组合成 <c>data-{name}</c> 格式的名称。</param>
    public HtmlDataAttribute([NotNull] string name) : base($"data-{name}")
    {
    }
}

/// <summary>
/// 为组件类的 HTML 元素定义指定的 role 属性。
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HtmlRoleAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 初始化 <see cref="HtmlRoleAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="value">HTML 元素中 role 属性的值。</param>
    public HtmlRoleAttribute([NotNull] string value) : base("role") => Value = value;
}
