namespace ComponentBuilder;

/// <summary>
/// Provides a function to component that can be refresh after state has changed.
/// </summary>
public interface IRefreshComponent
{
    /// <summary>
    /// Notify component's state has been changed.
    /// </summary>
    /// <returns></returns>
    Task NotifyStateChanged();
}
