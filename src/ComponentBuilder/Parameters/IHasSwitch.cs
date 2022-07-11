namespace ComponentBuilder.Parameters;
/// <summary>
/// Provides that a component can be switched to.
/// </summary>
public interface IHasSwitch
{
    /// <summary>
    /// The index to switch.
    /// </summary>
    int? SwitchIndex { get; set; }
}
