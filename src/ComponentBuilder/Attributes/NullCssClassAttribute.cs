namespace ComponentBuilder;

/// <summary>
/// Apply for component parameter when value is null to use specified CSS value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class NullCssClassAttribute : CssClassAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NullCssClassAttribute"/> class.
    /// </summary>
    /// <param name="css">The CSS value.</param>
    public NullCssClassAttribute(string? css) : base(css)
    {
    }
}
