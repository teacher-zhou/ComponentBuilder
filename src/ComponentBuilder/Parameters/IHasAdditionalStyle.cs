namespace ComponentBuilder.Parameters;
/// <summary>
/// Provides an addtional style of component to append.
/// </summary>
public interface IHasAdditionalStyle
{
    /// <summary>
    /// The style string to append.
    /// </summary>
    string? AdditionalStyle { get; set; }
}
