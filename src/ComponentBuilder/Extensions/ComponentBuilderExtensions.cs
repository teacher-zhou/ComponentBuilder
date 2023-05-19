using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// The extensions of ComponentBuilder.
/// </summary>
public static class ComponentBuilderExtensions
{
    /// <summary>
    /// 获取枚举成员定义了 <see cref="CssClassAttribute"/> 特性的 <see cref="CssClassAttribute.CSS"/> 的值。
    /// <para>
    /// 如果 <see cref="CssClassAttribute"/> 未定义，则返回带小写字符串的枚举成员名。
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">前缀字符串与返回字符串组合。</param>
    /// <param name="original">若使用枚举成员的原名称，则为 <c>true</c>，否则为 <c>false</c>。</param>
    /// <returns>CSS的值。</returns>
    public static string GetCssClass(this Enum @enum, string? prefix = default, bool original = default)
    {
        var enumType = @enum.GetType();

        if (enumType.TryGetCustomAttribute(out CssClassAttribute? attribute))
        {
            prefix += attribute!.CSS;
        }

        var enumMember = enumType.GetField(@enum.ToString());
        if (enumMember is null)
        {
            return string.Empty;
        }
        if (enumMember.TryGetCustomAttribute<CssClassAttribute>(out var cssClassAttribute))
        {
            return prefix + cssClassAttribute!.CSS;
        }
        return prefix + (original ? enumMember.Name : enumMember.Name.ToLower());
    }
    /// <summary>
    /// 获取枚举成员定义了 <see cref="HtmlAttributeAttribute"/> 特性的 HTML 属性字符串。
    /// <para>
    /// 如果<see cref="HtmlAttributeAttribute"/>未定义，则返回带小写字符串的成员名称。
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">前缀字符串与返回字符串组合。</param>
    /// <param name="original">若使用枚举成员的原名称，则为 <c>true</c>，否则为 <c>false</c>。</param>
    /// <returns>属性名称的值。</returns>
    public static string GetHtmlAttribute(this Enum @enum, string? prefix = default, bool original = default)
    {
        var enumType = @enum.GetType();

        var enumMember = enumType.GetField(@enum.ToString());
        if (enumMember is null)
        {
            return string.Empty;
        }
        if (enumMember.TryGetCustomAttribute<HtmlAttributeAttribute>(out var htmlAttribute))
        {
            return $"{prefix}{htmlAttribute!.Name}{htmlAttribute.Value}";
        }
        return $"{prefix}{(original ? enumMember.Name : enumMember.Name.ToLower())}";
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时，追加指定的 CSS 值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">要追加的CSS。</param>
    /// <param name="condition">决定要追加的值的条件。</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, Condition condition)
    {
        if (condition.Result)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// 追加指定的 CSS 集合字符串。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="values">要追加的一系列 CSS 的值。</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, IEnumerable<string> values)
    {
        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        foreach (var value in values)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 时，追加 <paramref name="trueValue"/> 作为 CSS 的值，否则追加 <paramref name="falseValue"/> 作为 CSS 的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">决定追加 <paramref name="trueValue"/> 或 <paramref name="falseValue"/> 的条件。</param>
    /// <param name="trueValue">当条件为 <c>true</c> 时追加的 CSS 值。</param>
    /// <param name="falseValue">当条件为 <c>false</c> 时追加的 CSS 值。</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string trueValue, Condition condition, string falseValue)
        => builder.Append(trueValue, condition).Append(falseValue, !condition.Result);


    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时，追加指定的 style 值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">要追加的 style。</param>
    /// <param name="condition">决定要追加的值的条件。</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, Condition condition)
    {
        if (condition.Result)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// 获取任意对象定义了 <see cref="CssClassAttribute"/> 特性的值。
    /// </summary>
    /// <param name="value">要获取的对象。</param>
    /// <returns><see cref="CssClassAttribute.CSS"/> 的值或 null。</returns>
    public static string? GetCssClass(this object value)
    {
        if ( value is Enum @enum )
        {
            return @enum.GetCssClass();
        }
        return value?.GetType().GetCustomAttribute<CssClassAttribute>()?.CSS;
    }
}
