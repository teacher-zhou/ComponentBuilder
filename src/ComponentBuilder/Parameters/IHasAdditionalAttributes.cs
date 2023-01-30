namespace ComponentBuilder.Parameters;
/// <summary>
/// Defines an additional attribute for component to capture unmatched html attribute values.
/// </summary>
public interface IHasAdditionalAttributes
{
    /// <summary>
    /// Gets or sets an additional attribute in an element that automatically captures unmatched html attribute values.
    /// </summary>
    IDictionary<string, object?> AdditionalAttributes { get; set; }
}
