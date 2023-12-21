namespace ComponentBuilder.Definitions;
/// <summary>
/// Define an additional attribute for the component to catch unmatched html attribute values.
/// </summary>
public interface IHasAdditionalAttributes
{
    /// <summary>
    /// Gets or sets an additional property in an element that automatically catches unmatched html property values.
    /// </summary>
    IDictionary<string, object?> AdditionalAttributes { get; set; }
}
