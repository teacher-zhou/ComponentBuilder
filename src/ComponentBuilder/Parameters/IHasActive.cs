namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides a parameter of component can be actived.
/// </summary>
public interface IHasActive
{
    /// <summary>
    /// Represents a status of component is actived.
    /// </summary>
    bool Active { get; set; }
}
