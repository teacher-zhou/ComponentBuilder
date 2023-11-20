namespace ComponentBuilder;

/// <summary>
/// Apply to <see cref="bool"/> parameter of component weither to use CSS when <c>true</c> or <c>false</c>.
/// </summary>
/// <seealso cref="CssClassAttribute" />
/// <param name="trueClass">Apply specified CSS string when parameter value is <c>true</c>.</param>
/// <param name="falseClass">Apply specified CSS string when parameter value is <c>false</c>. It can be default string.</param>
[AttributeUsage(AttributeTargets.Property)]
public class BooleanCssClassAttribute(string trueClass, string? falseClass = default) : CssClassAttribute
{
    /// <summary>
    /// Gets the CSS string when parameter is <c>true</c>.
    /// </summary>
    public string TrueClass { get; } = trueClass;
    /// <summary>
    /// Gets the CSS string when parameter is <c>false</c>.
    /// </summary>
    public string? FalseClass { get; } = falseClass;
}
