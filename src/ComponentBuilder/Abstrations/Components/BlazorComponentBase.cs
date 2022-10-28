namespace ComponentBuilder;
/// <summary>
/// Represents a base component class with all features.
/// </summary>
public abstract class BlazorComponentBase : BlazorAbstractComponentBase, IHasAdditionalCssClass, IHasAdditionalStyle, IHasCssClassUtility
{
    /// <summary>
    /// The extensions of <see cref="ICssClassUtility"/> instance.
    /// <para>
    /// Normally, you can define the extensions method of <see cref="ICssClassUtility"/> instance for your css class utilties.
    /// </para>
    /// </summary>
    [Parameter] public ICssClassUtility? CssClass { get; set; }
    /// <summary>
    /// Gets or sets the additional CSS class string by client. This value could append behind after all CSS from parameters finish building automatically.
    /// </summary>
    [Parameter] public string? AdditionalCssClass { get; set; }
    /// <summary>
    /// Gets or sets the additional style string by client. This value could append behind after all styles from parameters finish building automatically.
    /// </summary>
    [Parameter] public string? AdditionalStyle { get; set; }
}
