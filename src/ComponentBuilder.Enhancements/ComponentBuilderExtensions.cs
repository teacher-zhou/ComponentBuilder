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
    /// Return <typeparamref name="TAttribute"/> from field witch support two-way bindings.
    /// </summary>
    /// <typeparam name="TValue">The value type of field.</typeparam>
    /// <typeparam name="TAttribute">The attribute type of get.</typeparam>
    /// <param name="valueExpression">The expression of field.</param>
    /// <returns></returns>
    public static TAttribute? GetAttribute<TValue, TAttribute>(this Expression<Func<TValue>> valueExpression) where TAttribute : Attribute
    => ((MemberExpression)valueExpression!.Body)?.Member?.GetCustomAttribute<TAttribute>();

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

        if ( fieldInfo == null )
        {
            return enumName;
        }

        return fieldInfo.GetCustomAttribute<DefaultValueAttribute>()?.Value ?? enumName;
    }

    //public static bool IsAssignFrom(this object value,object target)
    //{
    //    if ( target is null )
    //    {
    //        throw new ArgumentNullException(nameof(target));
    //    }
    //    return target.GetType().IsAssignableFrom(value.GetType());
    //}

    //public static bool IsAssignFrom<TTarget>(this object value)
    //{
    //    return typeof(TTarget).IsAssignFrom(value);
    //}
}
