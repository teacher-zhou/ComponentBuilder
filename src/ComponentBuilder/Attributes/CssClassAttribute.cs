namespace ComponentBuilder;

/// <summary>
/// Apply the CSS value for component class, parameters, interface for pre-definition, enum members etc.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class CssClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="CssClassAttribute" /> class to apply parameter name.
    /// </summary>
    public CssClassAttribute() : this(null)
    {

    }

    /// <summary>
    /// Initializes a new instance of <see cref="CssClassAttribute"/> class by given css class name.
    /// </summary>
    /// <param name="name">The CSS class value.</param>
    public CssClassAttribute(string name) => Name = name;

    /// <summary>
    /// Gets CSS class value.
    /// </summary>
    public string Name { get; }
    /// <summary>
    /// Gets or sets order from small to large to create CSS class.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Disable to apply CSS value. 
    /// </summary>
    public bool Disabled { get; set; }
    /// <summary>
    /// <c>true</c> to append CSS value behind parameter value, otherwise, <c>false</c>.
    /// </summary>
    public bool Suffix { get; set; }
}