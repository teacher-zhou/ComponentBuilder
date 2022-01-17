namespace ComponentBuilder.Parameters;

/// <summary>
/// Defines a parameter representing disabled statement of component.
/// </summary>
public interface IHasDisabled
{
    /// <summary>
    /// Determined a state of component can be disabled.
    /// </summary>
    [CssClass("disabled")]bool Disabled { get; set; }
}
