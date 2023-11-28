namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides additional styles for the component to append.
/// </summary>
public interface IHasAdditionalStyle
{
    /// <summary>
    /// Gets or sets additional style strings to append.
    /// <para>
    /// Typically, this value is appended to the end of all style parameters after they have been built.
    /// </para>    
    /// </summary>
    string? AdditionalStyle { get; set; }
}
