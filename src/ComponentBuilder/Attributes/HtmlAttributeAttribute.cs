namespace ComponentBuilder;

/// <summary>
/// Represents an element attribute in component that applying by parameter value.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
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
    /// <param name="value">The value of name. <c>null</c> to use parameter value.</param>
    public HtmlAttributeAttribute(string name, object value = default)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Gets the name of atrribute.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets or sets attribute value.
    /// </summary>
    public object Value { get; set; }
}
