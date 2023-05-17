namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides a component with events that can be switched.
/// </summary>
public interface IHasOnSwitch : IHasSwitch, IHasEventCallback
{
    /// <summary>
    /// A switchable callback method for the specified component index.
    /// </summary>
    EventCallback<int?> OnSwitch { get; set; }
}
