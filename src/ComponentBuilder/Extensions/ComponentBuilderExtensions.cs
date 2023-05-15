using System.Reflection;

namespace ComponentBuilder.Automation;

/// <summary>
/// The extensions of ComponentBuilder.Automation.
/// </summary>
public static class ComponentBuilderExtensions
{
    /// <summary>
    /// Gets the <see cref="CssClassAttribute.CSS"/> value of <see cref="CssClassAttribute"/> definition for enum member. 
    /// <para>
    /// Returns the enum member name with lowercase string if <see cref="CssClassAttribute"/> is not defined.
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">The prefix string combine with return string.</param>
    /// <param name="original"><c>true</c> to use the original name of enum member; otherwise, <c>false</c>.</param>
    /// <returns>The value of CSS.</returns>
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
    /// Gets the <see cref="HtmlAttributeAttribute.Name"/> concat with <see cref="HtmlAttributeAttribute.Value"/> of <see cref="HtmlAttributeAttribute"/> attribute defined for enum member.
    /// <para>
    /// Returns the enum member name with lowercase string if <see cref="HtmlAttributeAttribute"/> is not defined.
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">The prefix string combine with return string.</param>
    /// <param name="original"><c>true</c> to use the original name of enum member; otherwise, <c>false</c>.</param>
    /// <returns>The value of attribute name.</returns>
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
    /// Append specified CSS value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">The CSS to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, Condition condition)
    {
        if (condition.Result)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Append speicified CSS collection string.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="values">The values to append.</param>
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
    /// Append <paramref name="trueValue"/> when <paramref name="condition"/> is <c>true</c>; otherwise append <paramref name="falseValue"/> value.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">A condition determines append <paramref name="trueValue"/> or <paramref name="falseValue"/>.</param>
    /// <param name="trueValue">Append when <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="falseValue">Append when <paramref name="condition"/> is <c>false</c>.</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string trueValue, Condition condition, string falseValue)
        => builder.Append(trueValue, condition).Append(falseValue, !condition.Result);


    /// <summary>
    /// Append speified style string when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">The style string to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, Condition condition)
    {
        if (condition.Result)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Append specified CSS value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">The CSS to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static ICssClassUtility Append(this ICssClassUtility builder, string value, Condition condition)
    {
        if (condition.Result )
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Gets the <see cref="CssClassAttribute.CSS"/> value from object defined <see cref="CssClassAttribute"/> attribute.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>The value of <see cref="CssClassAttribute.CSS"/> or null.</returns>
    public static string? GetCssClass(this object value)
    {
        if ( value is Enum @enum )
        {
            return @enum.GetCssClass();
        }
        return value?.GetType().GetCustomAttribute<CssClassAttribute>()?.CSS;
    }
}
