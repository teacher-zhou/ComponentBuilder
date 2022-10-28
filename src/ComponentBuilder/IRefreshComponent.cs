namespace ComponentBuilder;

/// <summary>
/// Provides the capability for components to refresh after state changes.
/// </summary>
public interface IRefreshableComponent
{
    /// <summary>
    /// Asynchorously notifies the component that its state has changed and rerenders the component.
    /// </summary>
    /// <returns>A task operation that does not contain a return value.</returns>
    Task NotifyStateChanged();
}
