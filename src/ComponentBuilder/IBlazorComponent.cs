namespace ComponentBuilder;

/// <summary>
/// Represents a razor component.
/// </summary>
public interface IBlazorComponent :IHasAdditionalAttributes, IComponent,IDisposable
{
    /// <summary>
    /// Get instance of <see cref="ICssClassBuilder"/>.
    /// </summary>
    ICssClassBuilder CssClassBuilder { get; }

    /// <summary>
    /// Get instance of <see cref="IStyleBuilder"/>.
    /// </summary>
    IStyleBuilder StyleBuilder { get; }
    /// <summary>
    /// Gets the child component collection.
    /// </summary>
    BlazorComponentCollection ChildComponents { get; }

    /// <summary>
    /// Asynchorously notifies the component that its state has changed and rerenders the component.
    /// </summary>
    /// <returns>A task operation that does not contain a return value.</returns>
    Task NotifyStateChanged();

    /// <summary>
    /// Get class string seperated by space for each item.
    /// </summary>
    /// <returns>A string seperated by space for each item or <c>null</c>. </returns>
    public string? GetCssClassString();
    /// <summary>
    /// Get style string seperated by semi-colon(;) for each item.
    /// </summary>
    /// <returns>A string seperated by semi-colon(;) for each item or <c>null</c>. </returns>
    public string? GetStyleString();
}
