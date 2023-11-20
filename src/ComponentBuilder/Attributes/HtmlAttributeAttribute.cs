using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// Represents parameter or component could generate HTML attribute.
/// </summary>
/// <param name="name">The name of HTML attribute. <c>Null</c> to use parameter name.</param>
/// <param name="value">The value of HTML attribute. <c>default</c> to use parameter value.</param>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
public class HtmlAttributeAttribute(string? name = default, object? value = default) : Attribute
{
    /// <summary>
    /// Gets the attribute name.
    /// </summary>
    public string? Name => name;
    /// <summary>
    /// Get the attribute value.
    /// </summary>
    public object? Value => value;
}


/// <summary>
/// Represents parameter or component could generate 'aria-{name}' HTML attribute.
/// </summary>
/// <param name="name">The name behind 'aria-'.</param>
/// <param name="value">The value of <paramref name="name"/>.</param>
public class HtmlAriaAttribute([NotNull] string name, object? value = default) : HtmlAttributeAttribute($"aria-{name}", value) { }

/// <summary>
/// Represents parameter or component could generate 'data-{name}' HTML attribute.
/// </summary>
/// <param name="name">The name behind 'data-'.</param>
/// <param name="value">The value of <paramref name="name"/>.</param>
public class HtmlDataAttribute([NotNull] string name, object? value = default) : HtmlAttributeAttribute($"data-{name}", value) { }

/// <summary>
/// Represents parameter or component could generate 'role' HTML attribute.
/// </summary>
/// <param name="value">The value of 'role' HTML attribute.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class HtmlRoleAttribute([NotNull] string value) : HtmlAttributeAttribute("role", value){}
