namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides a component with events that can be disabled.
/// </summary>
public interface IHasOnDisabled : IHasDisabled, IHasEventCallback
{
    /// <summary>
    /// A callback function when the component is disabled.
    /// </summary>
    EventCallback<bool> OnDisabled { get; set; }
}
