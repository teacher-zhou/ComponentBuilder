namespace ComponentBuilder;

/// <summary>
/// Represents a razor component.
/// </summary>
public interface IRazorComponent :IHasAdditionalAttributes, IComponent,IDisposable
{
    /// <summary>
    /// Asynchorously notifies the component that its state has changed and rerenders the component.
    /// </summary>
    /// <returns>A task operation that does not contain a return value.</returns>
    Task NotifyStateChanged();

    /// <summary>
    /// Get instance of <see cref="ICssClassBuilder"/>.
    /// </summary>
    ICssClassBuilder CssClassBuilder { get; }

    /// <summary>
    /// Get instance of <see cref="IStyleBuilder"/>.
    /// </summary>
    IStyleBuilder StyleBuilder { get; }
}
