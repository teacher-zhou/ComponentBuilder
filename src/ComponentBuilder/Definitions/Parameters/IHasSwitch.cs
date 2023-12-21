namespace ComponentBuilder.Definitions;
/// <summary>
/// Indicates that you can switch between components.
/// </summary>
public interface IHasSwitch
{
    /// <summary>
    /// Gets or sets the switching component index.
    /// </summary>
    int? SwitchIndex { get; set; }
}
