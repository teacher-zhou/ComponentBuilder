namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides an additional CSS class of component to append.
/// </summary>
public interface IHasAdditionalClass
{
    /// <summary>
    /// Gets or sets the additional CSS class string to append. 
    /// <para>
    /// Normally, this value could append behind after all CSS from parameters finish building.
    /// </para>
    /// </summary>
    string? AdditionalClass { get; set; }
}
