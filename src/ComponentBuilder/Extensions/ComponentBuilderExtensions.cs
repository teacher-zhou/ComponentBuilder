using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.JSInterop;

namespace ComponentBuilder;

/// <summary>
/// 组件扩展。
/// </summary>
public static class ComponentBuilderExtensions
{
    /// <summary>
    /// 尝试从 <see cref=" Type"/> 获取指定 <typeparamref name="TAttribute"/> 特性。
    /// </summary>
    /// <typeparam name="TAttribute">特性实例。</typeparam>
    /// <param name="type">The instance of type.</param>
    /// <param name="attribute">如果成功获取特性，则返回该实例，否则返回 <c>null</c>。</param>
    /// <param name="inherit">是否继承基类的特性。</param>
    /// <returns><c>true</c> 表示成功获取到指定的特性实例，否则为 <c>false</c> 。</returns>
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
    /// 尝试从 <see cref="FieldInfo"/> 获取指定 <typeparamref name="TAttribute"/> 特性。
    /// </summary>
    /// <typeparam name="TAttribute">特性实例。</typeparam>
    /// <param name="field">The instance of field.</param>
    /// <param name="attribute">如果成功获取特性，则返回该实例，否则返回 <c>null</c>。</param>
    /// <param name="inherit">是否继承基类的特性。</param>
    /// <returns><c>true</c> 表示成功获取到指定的特性实例，否则为 <c>false</c> 。</returns>
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
    /// 尝试从 <see cref="PropertyInfo"/> 获取指定 <typeparamref name="TAttribute"/> 特性。
    /// </summary>
    /// <typeparam name="TAttribute">特性实例。</typeparam>
    /// <param name="property">The instance of property.</param>
    /// <param name="attribute">如果成功获取特性，则返回该实例，否则返回 <c>null</c>。</param>
    /// <param name="inherit">是否继承基类的特性。</param>
    /// <returns><c>true</c> 表示成功获取到指定的特性实例，否则为 <c>false</c> 。</returns>
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
    /// 尝试从 <see cref="MethodInfo"/> 获取指定 <typeparamref name="TAttribute"/> 特性。
    /// </summary>
    /// <typeparam name="TAttribute">特性实例。</typeparam>
    /// <param name="method">The instance of method.</param>
    /// <param name="attribute">如果成功获取特性，则返回该实例，否则返回 <c>null</c>。</param>
    /// <param name="inherit">是否继承基类的特性。</param>
    /// <returns><c>true</c> 表示成功获取到指定的特性实例，否则为 <c>false</c> 。</returns>
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
    /// 返回枚举项定义了 <see cref="CssClassAttribute"/> 特性的 <see cref="CssClassAttribute.CSS"/> 的值，如果没有指定该特性，则返回枚举项的名称。
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">返回值要追加的前缀。</param>
    /// <param name="useOriginal">当枚举项没有定义 <see cref="CssClassAttribute"/> 特性时有效。若为 <c>true</c> 则保持枚举值的原始名称，否则使用小写字符串的枚举名称。</param>
    /// <returns>CSS 名称的字符串。</returns>
    public static string GetCssClass(this Enum @enum, string? prefix = default, bool useOriginal = default)
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
        return prefix + (useOriginal ? enumMember.Name : enumMember.Name.ToLower());
    }
    /// <summary>
    /// 返回枚举项定义了 <see cref="HtmlAttributeAttribute"/> 特性的 <see cref="HtmlAttributeAttribute.Name"/> 的值，如果没有指定该特性，则返回枚举项的名称。
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <param name="prefix">返回值要追加的前缀。</param>
    /// <param name="useOriginal">当枚举项没有定义 <see cref="HtmlAttributeAttribute"/> 特性时有效。若为 <c>true</c> 则保持枚举值的原始名称，否则使用小写字符串的枚举名称。</param>
    /// <returns>HTML 属性的名称。</returns>
    public static string GetHtmlAttribute(this Enum @enum, string? prefix = default, bool useOriginal = default)
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
        return prefix + (useOriginal ? enumMember.Name : enumMember.Name.ToLower());
    }
    /// <summary>
    /// 返回枚举项中定义了 <see cref="DefaultValueAttribute"/> 的 <see cref="DefaultValueAttribute.Value"/> 的值。
    /// </summary>
    /// <param name="enum">The instance of enum.</param>
    /// <returns>A value of <see cref="DefaultValueAttribute.Value"/> for member.</returns>
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
    /// <param name="disposing"><c>true</c> to dispose collection of builder, otherwise <c>false</c>.</param>
    /// <returns>A css class string separated by space for each item.</returns>
    internal static string? Build(this ICssClassBuilder builder, bool disposing)
    {
        var result = builder.ToString();
        if (disposing)
        {
            builder.Dispose();
        }
        return result;
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 追加 CSS 的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">要追加的值。</param>
    /// <param name="condition"><c>true</c> 时追加值。</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, bool condition)
    {
        if (condition)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// 追加指定字符串集合。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="values">要追加的值。</param>
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
    /// 当 <paramref name="condition"/> 是 <c>true</c> 追加 CSS 的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="value">要追加的值。</param>
    /// <param name="condition">一个委托，当返回 <c>true</c> 时追加值。</param>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, string value, Func<bool> condition)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        return builder.Append(value, condition());
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 时追加 <paramref name="trueValue"/> 的值，否则追加 <paramref name="falseValue"/> 的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">判断条件。</param>
    /// <param name="trueValue">当 <paramref name="condition"/> 是 <c>true</c> 时追加的值。</param>
    /// <param name="falseValue">当 <paramref name="condition"/> 是 <c>false</c> 时追加的值。</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, bool condition, string trueValue, string falseValue)
        => builder.Append(trueValue, condition).Append(falseValue, !condition);

    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 时追加 <paramref name="trueValue"/> 的值，否则追加 <paramref name="falseValue"/> 的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    /// <param name="condition">具备判断条件的委托。</param>
    /// <param name="trueValue">当 <paramref name="condition"/> 是 <c>true</c> 时追加的值。</param>
    /// <param name="falseValue">当 <paramref name="condition"/> 是 <c>false</c> 时追加的值。</param>
    /// <returns></returns>
    public static ICssClassBuilder Append(this ICssClassBuilder builder, Func<bool> condition, string trueValue, string falseValue)
    {
        if (condition is null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        return builder.Append(condition(), trueValue, falseValue);
    }


    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 追加指定的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">要追加的值。</param>
    /// <param name="condition">一个委托，当返回 <c>true</c> 时追加值。</param>
    /// <returns></returns>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, bool condition)
    {
        if (condition)
        {
            builder.Append(value);
        }
        return builder;
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 追加指定的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="value">要追加的值。</param>
    /// <param name="condition">一个委托，当返回 <c>true</c> 时追加值。</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string value, Func<bool> condition) => builder.Append(value, condition());

    /// <summary>
    /// 追加指定的字符串格式以合成样式字符串。
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="format">复合格式字符串。</param>
    /// <param name="args">包含零个或多个要格式化的对象的对象数组。</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string format, params object?[] args)
        => builder.Append(string.Format(format, args));

    /// <summary>
    /// 附加指定的名称和值以合成样式字符串。
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    /// <param name="name">样式名称。</param>
    /// <param name="value">样式的值。</param>
    public static IStyleBuilder Append(this IStyleBuilder builder, string name, object? value)
        => builder.Append(string.Concat(name, ":", value));

    /// <summary>
    /// 以异步的方式导入指定路径的 javascript 模块。
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="contentPath">要导入的 JS 模块的路径。</param>
    /// <returns>表示 javascript 模块的任务。</returns>
    public static async Task<dynamic> Import(this IJSRuntime js, string contentPath)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", contentPath);
        return new DynamicJsReferenceObject(module);
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 是 <c>true</c> 追加 CSS 的值。
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassUtility"/>.</param>
    /// <param name="value">要追加的值。</param>
    /// <param name="condition"><c>true</c> 时追加值。</param>
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
}
