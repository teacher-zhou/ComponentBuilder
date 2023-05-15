namespace ComponentBuilder.Automation;

/// <summary>
/// Define for component to apply CSS class when value is set.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
public class CssClassAttribute : Attribute
{
    /// <summary>
    /// Use same name of parameter to initializes a new instance of the <see cref="CssClassAttribute"/> class.
    /// <para>
    /// NOTE: Only worked for <c>[Parameter]</c> property.
    /// </para>
    /// </summary>
    public CssClassAttribute() : this(default)
    {

    }

    /// <summary>
    /// Use spcified CSS class value to initializes a new instance of the <see cref="CssClassAttribute"/> class.
    /// </summary>
    /// <param name="css">The CSS class string.</param>
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
    /// Gets or sets a boolean value that prohibits CSS values from being applied to the current parameter.
    /// </summary>
    public bool Disabled { get; set; }

    /// <summary>
    /// Gets or sets a boolean value that can concate the <see cref="CSS"/> value from base component.
    /// <para>
    /// NOTE: This value worked ONLY for component class.
    /// </para>
    /// </summary>
    public bool Concat { get; set; }
}