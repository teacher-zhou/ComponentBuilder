namespace ComponentBuilder;

/// <summary>
/// 应用于布尔值的参数 <see cref="Boolean"/> 的 CSS 名称。
/// </summary>
/// <seealso cref="CssClassAttribute" />
[AttributeUsage(AttributeTargets.Property)]
public class BooleanCssClassAttribute : CssClassAttribute
{
    /// <summary>
    /// 初始化 <see cref="BooleanCssClassAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="trueCssClass">当参数值是 <c>true</c> 时应用的 CSS 名称。</param>
    /// <param name="falseCssClass">当参数值 <c>false</c> 时应用的 CSS 名称。</param>
    public BooleanCssClassAttribute(string trueCssClass, string? falseCssClass = default)
    {
        TrueCssClass = trueCssClass;
        FalseCssClass = falseCssClass;
    }
    /// <summary>
    /// Gets the true CSS class when value is <c>true</c>.
    /// </summary>
    public string TrueCssClass { get; }
    /// <summary>
    /// Gets the true CSS class when value is <c>false</c>.
    /// </summary>
    public string? FalseCssClass { get; }
}
