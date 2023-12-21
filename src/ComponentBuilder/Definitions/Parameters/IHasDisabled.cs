namespace ComponentBuilder.Definitions;

/// <summary>
/// Defines a parameter that has disabled HTML attribute.
/// </summary>
public interface IHasDisabled
{
    /// <summary>
    /// Set true to generate HTML 'disabled' attribute.
    /// </summary>
    [HtmlAttribute] bool Disabled { get; set; }
}
