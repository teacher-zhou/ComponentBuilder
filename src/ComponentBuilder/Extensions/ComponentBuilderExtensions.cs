using ComponentBuilder.Builder;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// The extensions of ComponentBuilder.
/// </summary>
public static class ComponentBuilderExtensions
{
    /// <summary>
    /// Try to get <typeparamref name="TAttribute"/> attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="type">The instance of type.</param>
    /// <param name="attribute">Returns the attribute or <c>null</c>.</param>
    /// <param name="inherit">true to inspect the ancestors of element; otherwise, false.</param>
    /// <returns><c>true</c> for <paramref name="attribute"/> is not null; otherwise, <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<TAttribute>(this Type type, out TAttribute? attribute, bool inherit = default) where TAttribute : Attribute
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        attribute = type.GetCustomAttribute<TAttribute>(inherit);
        return attribute != null;
    }
    /// <summary>
    /// Try to get <typeparamref name="TAttribute"/> attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="field">The instance of field.</param>
    /// <param name="attribute">Returns the attribute or <c>null</c>.</param>
    /// <param name="inherit">true to inspect the ancestors of element; otherwise, false.</param>
    /// <returns><c>true</c> for <paramref name="attribute"/> is not null; otherwise, <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<TAttribute>(this FieldInfo field, out TAttribute? attribute, bool inherit = default) where TAttribute : Attribute
    {
        if (field is null)
        {
            throw new ArgumentNullException(nameof(field));
        }

        attribute = field.GetCustomAttribute<TAttribute>(inherit);
        return attribute != null;
    }
    /// <summary>
    /// Try to get <typeparamref name="TAttribute"/> attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="property">The instance of property.</param>
    /// <param name="attribute">Returns the attribute or <c>null</c>.</param>
    /// <param name="inherit">true to inspect the ancestors of element; otherwise, false.</param>
    /// <returns><c>true</c> for <paramref name="attribute"/> is not null; otherwise, <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<TAttribute>(this PropertyInfo property, out TAttribute? attribute, bool inherit = default) where TAttribute : Attribute
    {
        if (property is null)
        {
            throw new ArgumentNullException(nameof(property));
        }

        attribute = property.GetCustomAttribute<TAttribute>(inherit);
        return attribute != null;
    }

    /// <summary>
    /// Try to get <typeparamref name="TAttribute"/> attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="method">The instance of method.</param>
    /// <param name="attribute">Returns the attribute or <c>null</c>.</param>
    /// <param name="inherit">true to inspect the ancestors of element; otherwise, false.</param>
    /// <returns><c>true</c> for <paramref name="attribute"/> is not null; otherwise, <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<TAttribute>(this MethodInfo method, out TAttribute? attribute, bool inherit = default) where TAttribute : Attribute
    {
        if (method is null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        attribute = method.GetCustomAttribute<TAttribute>(inherit);
        return attribute != null;
    }
    /// <summary>
    /// Try to get <typeparamref name="TAttribute"/> attribute.
    /// </summary>
    /// <typeparam name="TAttribute">The type of attribute.</typeparam>
    /// <param name="member">The instance of member.</param>
    /// <param name="attribute">Returns the attribute or <c>null</c>.</param>
    /// <param name="inherit">true to inspect the ancestors of element; otherwise, false.</param>
    /// <returns><c>true</c> for <paramref name="attribute"/> is not null; otherwise, <c>false</c>.</returns>
    public static bool TryGetCustomAttribute<TAttribute>(this MemberInfo member, out TAttribute? attribute, bool inherit = default) where TAttribute : Attribute
    {
        if ( member is null )
        {
            throw new ArgumentNullException(nameof(member));
        }

        attribute = member.GetCustomAttribute<TAttribute>(inherit);
        return attribute != null;
    }

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
    /// Gets the <see cref="HtmlAttributeAttribute.Name"/> of <see cref="HtmlAttributeAttribute"/> attribute defined for enum member.
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
        if (enumMember.TryGetCustomAttribute<HtmlAttributeAttribute>(out var cssClassAttribute))
        {
            return prefix + cssClassAttribute!.Name;
        }
        return prefix + (original ? enumMember.Name : enumMember.Name.ToLower());
    }
    /// <summary>
    /// Gets the <see cref="DefaultValueAttribute.Value"/> of <see cref="DefaultValueAttribute"/> defined for enum member.
    /// <para>
    /// Returns the enum member name with lowercase string if <see cref="DefaultValueAttribute"/> is not defined.
    /// </para>
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <returns>A value of <see cref="DefaultValueAttribute.Value"/> for enum member.</returns>
    public static object? GetDefaultValue(this Enum @enum)
    {
        var enumType = @enum.GetType();
        var enumName = @enum.ToString().ToLower();
        var fieldInfo = enumType.GetTypeInfo().GetDeclaredField(@enum.ToString());

        if (fieldInfo == null)
        {
            return enumName;
        }

        var attr = fieldInfo.GetCustomAttribute<DefaultValueAttribute>();
        return attr == null ? enumName : attr!.Value;
    }

    /// <summary>
    /// Build css class string and dispose builder collection.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="clear"><c>true</c> to clear collection of builder after <see cref="ICssClassBuilder.ToString"/> is called, otherwise <c>false</c>.</param>
    /// <returns>A css class string separated by space for each item.</returns>
    internal static string? Build(this ICssClassBuilder builder, bool clear)
    {
        var result = builder.ToString();
        if (clear)
        {
            builder.Clear();
        }
        return result;
    }

    /// <summary>
    /// Append specified CSS value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">The CSS to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, bool condition)
    {
        if (condition)
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
    /// Append specified CSS value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">The CSS to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, Func<bool> condition)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        return builder.Append(value, condition());
    }

    /// <summary>
    /// Append <paramref name="trueValue"/> when <paramref name="condition"/> is <c>true</c>; otherwise append <paramref name="falseValue"/> value.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">A condition determines append <paramref name="trueValue"/> or <paramref name="falseValue"/>.</param>
    /// <param name="trueValue">Append when <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="falseValue">Append when <paramref name="condition"/> is <c>false</c>.</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string trueValue, bool condition, string falseValue)
        => builder.Append(trueValue, condition).Append(falseValue, !condition);

    /// <summary>
    /// Append <paramref name="trueValue"/> when <paramref name="condition"/> is <c>true</c>; otherwise append <paramref name="falseValue"/> value.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">A condition determines to append <paramref name="trueValue"/> or <paramref name="falseValue"/>.</param>
    /// <param name="trueValue">Append when <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="falseValue">Append when <paramref name="condition"/> is <c>false</c>.</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string trueValue, Func<bool> condition, string falseValue)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        return builder.Append(trueValue, condition(),  falseValue);
    }


    /// <summary>
    /// Append speified style string when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">The style string to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, bool condition)
    {
        if (condition)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Append speified style string when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">The style string to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, Func<bool> condition) => builder.Append(value, condition());

    /// <summary>
    /// Append specified CSS value when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">The CSS to append.</param>
    /// <param name="condition">A condition determines value to append.</param>
    public static ICssClassUtility Append(this ICssClassUtility builder, string value, bool condition)
    {
        if (condition)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// Return <typeparamref name="TAttribute"/> from field witch support two-way bindings.
    /// </summary>
    /// <typeparam name="TValue">The value type of field.</typeparam>
    /// <typeparam name="TAttribute">The attribute type of get.</typeparam>
    /// <param name="valueExpression">The expression of field.</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TValue, TAttribute>(this Expression<Func<TValue>> valueExpression) where TAttribute : Attribute
    => ((MemberExpression)valueExpression!.Body)?.Member?.GetCustomAttribute<TAttribute>();

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
