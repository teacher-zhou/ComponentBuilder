namespace ComponentBuilder;

/// <summary>
/// 应用于组件类。表示生成 HTML 元素中的属性名称为 <c>role</c> 。
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HtmlRoleAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 使用指定的值初始化 <see cref="HtmlRoleAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="value">HTML 属性是 <c>role</c> 的值。</param>
    public HtmlRoleAttribute(object value) : base("role")
    {
        Value = value;
    }
}
