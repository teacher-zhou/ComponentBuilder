using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// The extensions of ComponentBuilder.
/// </summary>
public static class ComponentBuilderExtensions
{
    /// <summary>
    /// Gets the value of <see cref="CssClassAttribute.CSS"/> for the enumeration member that defines the attribute <see cref=" CssClassAttribute.CSS "/>.
    /// <para>
    /// If <see cref="CssClassAttribute"/> is not defined, return the enumeration member name with a lowercase string.
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">The prefix string is combined with the return string.</param>
    /// <param name="original">If the original name of the enumeration member is used, it is <c>true</c>, otherwise it is <c>false</c>.</param>    <returns>The value of CSS name.</returns>
    [Obsolete("The GetCssClass will be removed in next version, Use GetCssClassAttribute instead")]
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
    /// Gets the value of <see cref="CssClassAttribute.CSS"/> for the enumeration member that defines the attribute <see cref=" CssClassAttribute.CSS "/>.
    /// <para>
    /// If <see cref="CssClassAttribute"/> is not defined, return the enumeration member name with a lowercase string.
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">The prefix string is combined with the return string.</param>
    /// <param name="original">If the original name of the enumeration member is used, it is <c>true</c>, otherwise it is <c>false</c>.</param>    <returns>The value of CSS name.</returns>
    public static string GetCssClassAttribute(this Enum @enum, string? prefix = default, bool original = default)
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
    /// Gets the HTML attribute string of the enumeration member that defines the <see cref="HtmlAttributeAttribute"/> attribute.
    /// <para>
    /// If <see cref="HtmlAttributeAttribute"/> is not defined, return the member name with a lowercase string.
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">The prefix string is combined with the return string.</param>
    /// <param name="original">If the original name of the enumeration member is used, it is <c>true</c>, otherwise it is <c>false</c>.</param>
    /// <returns>The value of the property name.</returns>
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
    /// Append the specified CSS value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">CSS to append.</param>
    /// <param name="condition">Conditions that determine the value to append.</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, Condition condition)
    {
        if (condition.Result)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Appends the specified CSS collection string.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="values">A list of CSS values to append.</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, IEnumerable<string> values) => builder.Append(values.ToArray());
    /// <summary>
    /// Appends the specified CSS collection string.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="values">A list of CSS values to append.</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, params string[] values)
    {
        ArgumentNullException.ThrowIfNull(values);

        foreach (var value in values)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Append <paramref name="trueValue"/> as the CSS value when <paramref name="condition"/> is <c>true</c>. Otherwise append <paramref name="falseValue"/> as a value of CSS.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">Decides to append the condition <paramref name="trueValue"/> or <paramref name="falseValue"/>.</param>
    /// <param name="trueValue">The CSS value appended when the condition is <c>true</c>.</param>
    /// <param name="falseValue">The CSS value appended when the condition is <c>false</c>.</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string trueValue, Condition condition, string falseValue)
        => builder.Append(trueValue, condition).Append(falseValue, !condition.Result);


    /// <summary>
    /// Append the specified style value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">style to append.</param>
    /// <param name="condition">Conditions that determine the value to append.</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, Condition condition)
    {
        if (condition.Result)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Gets the value of any object that defines the <see cref="CssClassAttribute"/> attribute.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns><see cref="CssClassAttribute.CSS"/> string or null。</returns>
    public static string? GetCssClass(this object value)
    {
        if ( value is Enum @enum )
        {
            return @enum.GetCssClass();
        }
        return value?.GetType().GetCustomAttribute<CssClassAttribute>()?.CSS;
    }
}
