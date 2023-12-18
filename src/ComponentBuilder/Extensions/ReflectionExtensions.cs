using System.Linq.Expressions;
using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// The extensions of ComponentBuilder.
/// </summary>
public static class ReflectionExtensions
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
        ArgumentNullException.ThrowIfNull(type);

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
        ArgumentNullException.ThrowIfNull(field);

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
        ArgumentNullException.ThrowIfNull(property);

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
        ArgumentNullException.ThrowIfNull(method);

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
        ArgumentNullException.ThrowIfNull(member);

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
}
