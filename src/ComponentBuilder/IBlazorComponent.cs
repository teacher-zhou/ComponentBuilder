namespace ComponentBuilder;

/// <summary>
/// Represents a blazor component.
/// </summary>
public interface IBlazorComponent : IHasAdditionalAttributes, IComponent, IDisposable
{
    ///// <summary>
    ///// Gets the reference of element.
    ///// </summary>
    //ElementReference? Reference { get; }

    ///// <summary>
    ///// Gets the CSS class builder.
    ///// </summary>
    //ICssClassBuilder CssClassBuilder { get; }

    ///// <summary>
    ///// Gets the style builder.
    ///// </summary>
    //IStyleBuilder StyleBuilder { get; }
    /// <summary>
    /// Gets the child components.
    /// </summary>
    BlazorComponentCollection ChildComponents { get; }

    /// <summary>
    /// Asynchronously notifies the component that its state has changed and renders the component.
    /// </summary>
    Task NotifyStateChanged();

    ///// <summary>
    ///// Gets a space-separated class string for each item.
    ///// </summary>
    ///// <returns>A string separated by Spaces, each entry or <c>null</c>.</returns>
    //public string? GetCssClassString();
    ///// <summary>
    ///// Get each item with a semicolon (;) Delimited style string.
    ///// </summary>
    ///// <returns>Use a semicolon (;) Delimited strings, each item or <c>null</c>.</returns>
    //public string? GetStyleString();

    ///// <summary>
    ///// Returns the HTML element name.
    ///// </summary>
    ///// <returns>The string represents the HTML element tag name.</returns>
    ///// <exception cref="ArgumentException">The value is <c>null</c> or an empty string.</exception>
    //string GetTagName();

    ///// <summary>
    ///// Build components with automatic features.
    ///// </summary>
    ///// <param name="builder"><paramref name="builder"/> instance to build.</param>
    //void BuildComponent(RenderTreeBuilder builder);

    /// <summary>
    /// Returns the properties of the component.
    /// </summary>
    /// <returns>Key/value pairs containing HTML attributes.</returns>
    IEnumerable<KeyValuePair<string, object?>> GetAttributes();
}
