namespace ComponentBuilder;

/// <summary>
/// Define for component to apply CSS class when value is set.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class CssClassAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CssClassAttribute"/> class.
    /// <para>
    /// Only apply CSS same as parameter name if is parameter.
    /// </para>
    /// </summary>
    public CssClassAttribute() : this(default)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CssClassAttribute"/> class.
    /// </summary>
    /// <param name="css">The CSS value.</param>
    public CssClassAttribute(string? css) => CSS = css;

    /// <summary>
    /// Gets CSS value.
    /// </summary>
    public string? CSS { get; }
    /// <summary>
    /// Gets or sets the CSS values in order, from least to most.
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// Gets or sets a Boolean value that prohibits CSS values from being applied to the current parameter.
    /// </summary>
    public bool Disabled { get; set; }
}