namespace ComponentBuilder;

/// <summary>
/// Represents an 'data-' attribute of element for applying by parameter value.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface)]
public class HtmlDataAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="HtmlDataAttribute"/> class by given data name and value.
    /// </summary>
    /// <param name="name">The data name, like 'data-{name}' to build attribute.</param>
    public HtmlDataAttribute(string name) : base($"data-{name}")
    {
    }
}
