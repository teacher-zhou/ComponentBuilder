namespace ComponentBuilder;

/// <summary>
/// 用于 <see cref="bool"/> 类型的组件参数，定义布尔值时所应用到组件上的 class 字符串。
/// </summary>
/// <seealso cref="CssClassAttribute" />
[AttributeUsage(AttributeTargets.Property)]
public class BooleanCssClassAttribute : CssClassAttribute
{

    /// <summary>
    /// 初始化 <see cref="BooleanCssClassAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="trueClass">当参数的值是 <c>true</c> 时应用的 class 字符串。</param>
    /// <param name="falseClass">当参数的值是 <c>false</c> 时应用的 class 字符串。</param>
    public BooleanCssClassAttribute(string? trueClass, string? falseClass = default)
    {
        TrueClass = trueClass;
        FalseClass = falseClass;
    }
    /// <summary>
    /// 获取当参数的值是 <c>true</c> 时的 class 字符串。
    /// </summary>
    public string? TrueClass { get; }
    /// <summary>
    /// 获取当参数的值是 <c>false</c> 时的 class 字符串。
    /// </summary>
    public string? FalseClass { get; }
}
