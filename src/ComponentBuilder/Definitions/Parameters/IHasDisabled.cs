namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides a parameter of component can be disabled.
/// </summary>
public interface IHasDisabled
{
    /// <summary>
    /// Represents a status of component is disabled.
    /// </summary>
    [HtmlAttribute] bool Disabled { get; set; }
}
