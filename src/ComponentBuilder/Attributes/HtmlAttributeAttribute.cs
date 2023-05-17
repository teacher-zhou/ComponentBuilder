namespace ComponentBuilder;

/// <summary>
/// Define for component generate HTML attribute when value is set.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false,Inherited =true)]
public class HtmlAttributeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlAttributeAttribute"/> class.
    /// </summary>
    public HtmlAttributeAttribute() : this(null)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlAttributeAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of HTML attribute.</param>
    public HtmlAttributeAttribute(string? name) => Name = name;

    /// <summary>
    /// Gets name of attribute.
    /// </summary>
    public string? Name { get; }
    /// <summary>
    /// Gets or sets the fixed value of attribute.
    /// </summary>
    public string? Value { get; set; }
}
