namespace ComponentBuilder;

/// <summary>
/// 当组件参数的值是 <c>null</c> 时使用的 class 字符串。
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NullCssClassAttribute : CssClassAttribute
{
    /// <summary>
    /// 初始化 <see cref="NullCssClassAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="css">CSS 字符串。</param>
    public NullCssClassAttribute(string? css) : base(css)
    {
    }
}
