namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides an addtional style of component to append.
/// </summary>
public interface IHasAdditionalStyle
{
    /// <summary>
    /// Gets or sets the additional style string to append. 
    /// <para>
    /// Normally, this value could append behind after all styles from parameters finish building.
    /// </para>    
    /// </summary>
    string? AdditionalStyle { get; set; }
}
