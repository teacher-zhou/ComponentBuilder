using System.Linq;
using System.Text;

using ComponentBuilder.Abstrations.Internal;

namespace ComponentBuilder;

/// <summary>
/// The helper for html.
/// </summary>
public static class HtmlHelper
{
    /// <summary>
    /// Merge html attribtes.
    /// </summary>
    /// <param name="htmlAttributes">The html attributes.</param>
    /// <returns>A key/value pairs of attributes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="htmlAttributes"/> is <c>null</c>.</exception>
    public static IEnumerable<KeyValuePair<string, object>> MergeHtmlAttributes(object htmlAttributes)
    {
        if (htmlAttributes is null)
        {
            throw new ArgumentNullException(nameof(htmlAttributes));
        }

        if (htmlAttributes is IEnumerable<KeyValuePair<string, object>> keyValueAttributes)
        {
            return keyValueAttributes;
        }

        return htmlAttributes.GetType().GetProperties()
            .Select(property =>
            {
                var name = property.Name.Replace("_", "-");
                var value = property.GetValue(htmlAttributes);
                return new KeyValuePair<string, object>(name, value);
            }).Distinct();
    }

    /// <summary>
    /// Returns css class that is satisfied by condition.
    /// </summary>
    /// <param name="condition">A condition that is satisfied.</param>
    /// <param name="trueCssClass">A CSS class string weither <paramref name="condition"/> is <c>true</c>.</param>
    /// <param name="falseCssClass">A CSS class string weither <paramref name="condition"/> is <c>false</c>.</param>
    /// <param name="prepend">A fixed string to prepend to final CSS class with a space seperator when this value is not null or empty.</param>
    /// <returns>A CSS class string.</returns>
    public static string CreateCssClass(bool condition, string trueCssClass, string? falseCssClass = default, string? prepend = default)
    {
        var builder = new StringBuilder();
        if (!string.IsNullOrEmpty(prepend))
        {
            builder.Append(prepend).Append(' ');
        }
        return builder.Append(condition ? trueCssClass : falseCssClass).ToString();
    }

    /// <summary>
    /// Create CSS class builder.
    /// </summary>
    /// <returns>A new instance of <see cref="DefaultCssClassBuilder"/> class.</returns>
    public static ICssClassBuilder CreateCssBuilder() => new DefaultCssClassBuilder();

    /// <summary>
    /// Create style builder.
    /// </summary>
    /// <returns>A new instance of <see cref="DefaultStyleBuilder"/> class.</returns>
    public static IStyleBuilder CreateStyleBuilder()=>new DefaultStyleBuilder();

    /// <summary>
    /// Create <see cref="EventCallback"/> for attribute of component.
    /// </summary>
    /// <typeparam name="TValue">The type of argumnt for callback action.</typeparam>
    /// <param name="receiver">The object of event callback trigger.</param>
    /// <param name="callback">The callback action to execute.</param>
    /// <param name="condition">Set <c>true</c> to create callback, otherwise <c>false</c>.</param>
    /// <returns>A bound event handler delegate</returns>
    public static EventCallback<TValue> CreateCallback<TValue>(object receiver,Action<TValue> callback,bool condition=true)
    {
        if (condition)
        {
            return EventCallback.Factory.Create(receiver, callback);
        }

        return default;
    }

    /// <summary>
    /// Create <see cref="EventCallback"/> for attribute of component.
    /// </summary>
    /// <param name="receiver">The object of event callback trigger.</param>
    /// <param name="callback">The callback action to execute.</param>
    /// <param name="condition">Set <c>true</c> to create callback, otherwise <c>false</c>.</param>
    /// <returns>A bound event handler delegate</returns>
    public static EventCallback CreateCallback(object receiver, Action callback, bool condition = true)
    {
        if (condition)
        {
            return EventCallback.Factory.Create(receiver, callback);
        }

        return default;
    }

    /// <summary>
    /// Create <see cref="EventCallback"/> for attribute of component.
    /// </summary>
    /// <typeparam name="TValue">The type of argumnt for callback action.</typeparam>
    /// <param name="receiver">The object of event callback trigger.</param>
    /// <param name="callback">The callback function to execute.</param>
    /// <param name="condition">Set <c>true</c> to create callback, otherwise <c>false</c>.</param>
    /// <returns>A bound event handler delegate</returns>
    public static EventCallback<TValue> CreateCallback<TValue>(object receiver, Func<TValue,Task> callback, bool condition = true)
    {
        if (condition)
        {
            return EventCallback.Factory.Create(receiver, callback);
        }

        return default;
    }

    /// <summary>
    /// Create <see cref="EventCallback"/> for attribute of component.
    /// </summary>
    /// <param name="receiver">The object of event callback trigger.</param>
    /// <param name="callback">The callback function to execute.</param>
    /// <param name="condition">Set <c>true</c> to create callback, otherwise <c>false</c>.</param>
    /// <returns>A bound event handler delegate</returns>
    public static EventCallback CreateCallback(object receiver, Func<Task> callback, bool condition = true)
    {
        if (condition)
        {
            return EventCallback.Factory.Create(receiver, callback);
        }

        return default;
    }
}
