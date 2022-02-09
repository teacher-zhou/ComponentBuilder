namespace ComponentBuilder;

/// <summary>
/// Represents an element attribute in component that applying by parameter value.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false)]
public class HtmlAttributeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="HtmlAttributeAttribute"/> class.
    /// </summary>
    public HtmlAttributeAttribute() : this(null)
    {

    }
    /// <summary>
    /// Initializes a new instance of <see cref="HtmlAttributeAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of attribute for html tag.</param>
    public HtmlAttributeAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of atrribute, <c>null</c> to use parameter property name with lowercase.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets or sets attribute value of name, <c>null</c> to use parameter value with lowercase for none numberic value.
    /// </summary>
    public object Value { get; set; }
}
