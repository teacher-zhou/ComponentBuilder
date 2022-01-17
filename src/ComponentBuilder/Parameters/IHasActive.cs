namespace ComponentBuilder.Parameters;

/// <summary>
/// Defines a parameter representing active statement of component.
/// </summary>
public interface IHasActive
{
    /// <summary>
    /// Determined a state of component can be actived.
    /// </summary>
    [CssClass("active")] bool Active { get; set; }
}
