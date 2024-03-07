namespace ComponentBuilder;

/// <summary>
/// Represents a blazor component.
/// </summary>
public interface IBlazorComponent : IHasAdditionalAttributes, IComponent, IDisposable
{
    /// <summary>
    /// Gets the child components.
    /// </summary>
    BlazorComponentCollection ChildComponents { get; }

    /// <summary>
    /// Asynchronously notifies the component that its state has changed and renders the component.
    /// </summary>
    Task NotifyStateChanged();

    /// <summary>
    /// Returns the properties of the component.
    /// </summary>
    /// <returns>Key/value pairs containing HTML attributes.</returns>
    IEnumerable<KeyValuePair<string, object?>> GetAttributes();
}
