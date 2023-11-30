namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides components with switchable events.
/// </summary>
public interface IHasOnSwitch : IHasSwitch, IHasEventCallback
{
    /// <summary>
    /// Specifies the switchable callback method for the component index.
    /// </summary>
    EventCallback<int?> OnSwitch { get; set; }
}
