namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides a component with deactivated events.
/// </summary>
public interface IHasOnDisabled : IHasDisabled, IHasEventCallback
{
    /// <summary>
    /// A callback function when a component is disabled.
    /// </summary>
    EventCallback<bool> OnDisabled { get; set; }
}
