namespace ComponentBuilder;

/// <summary>
/// Define a CSS class only Applied for <see cref="Boolean"/> parameters.
/// </summary>
/// <seealso cref="CssClassAttribute" />
[AttributeUsage(AttributeTargets.Property)]
public class BooleanCssClassAttribute : CssClassAttribute
{

    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanCssClassAttribute"/> class.
    /// </summary>
    /// <param name="trueCssClass">Apply CSS string when parameter value is <c>true</c>.</param>
    /// <param name="falseCssClass">Apply CSS string when parameter value is <c>false</c>.</param>
    public BooleanCssClassAttribute(string trueCssClass, string? falseCssClass = default)
    {
        TrueCssClass = trueCssClass;
        FalseCssClass = falseCssClass;
    }
    /// <summary>
    /// Gets the true CSS class when value is <c>true</c>.
    /// </summary>
    public string TrueCssClass { get; }
    /// <summary>
    /// Gets the true CSS class when value is <c>false</c>.
    /// </summary>
    public string? FalseCssClass { get; }
}
