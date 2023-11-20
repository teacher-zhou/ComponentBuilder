using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// Specified parameter a CSS string to generate when value is <c>null</c>.
/// </summary>
/// <param name="cssClass">The CSS class string when parameter value is <c>null</c>.</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NullCssClassAttribute([NotNull]string cssClass) : CssClassAttribute(cssClass){}
