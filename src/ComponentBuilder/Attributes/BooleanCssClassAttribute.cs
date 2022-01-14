namespace ComponentBuilder;

/// <summary>
/// Apply the CSS value for component parameter which is <see cref="Boolean"/> type.
/// </summary>
/// <seealso cref="CssClassAttribute" />
[AttributeUsage(AttributeTargets.Property)]
public class BooleanCssClassAttribute : CssClassAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanCssClassAttribute"/> class.
    /// </summary>
    /// <param name="trueCssClass">Apply CSS class value when value of property is <c>true</c>.</param>
    /// <param name="falseCssClass">Apply CSS class value when value of property is <c>true</c>.</param>
    public BooleanCssClassAttribute(string trueCssClass, string falseCssClass)
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
    public string FalseCssClass { get; }
}
