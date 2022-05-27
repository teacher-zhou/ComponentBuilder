namespace ComponentBuilder;

/// <summary>
/// Provides a function to component that can be refresh after state has changed.
/// </summary>
public interface IRefreshableComponent
{
    /// <summary>
    /// Notify the state of component has been changed and re-render component.
    /// </summary>
    /// <returns>A task operator represents contains no return value.</returns>
    Task NotifyStateChanged();
}
