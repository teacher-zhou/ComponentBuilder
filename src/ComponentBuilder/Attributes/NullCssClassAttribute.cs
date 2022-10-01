namespace ComponentBuilder;

/// <summary>
/// 应用于组件的参数，当参数值是 <c>null</c> 时应用的 CSS 类名称。
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NullCssClassAttribute : CssClassAttribute
{
    /// <summary>
    /// 使用指定的 CSS 名称初始化 <see cref="NullCssClassAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">CSS 名称。</param>
    public NullCssClassAttribute(string? name) : base(name)
    {
    }
}
