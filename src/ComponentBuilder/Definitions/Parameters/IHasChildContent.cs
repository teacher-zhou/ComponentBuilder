namespace ComponentBuilder.Definitions;

/// <summary>
/// Provide UI fragments for components to support child content.
/// </summary>
public interface IHasChildContent
{
    /// <summary>
    /// Gets or sets a fragment of UI content.
    /// </summary>
    RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// Provide UI fragments for components to support child content with specified value.
/// </summary>
/// <typeparam name="TValue">The type of value。</typeparam>
public interface IHasChildContent<TValue>
{
    /// <summary>
    /// Gets or sets a fragment of UI content with <typeparamref name="TValue"/> type of value.
    /// </summary>
    RenderFragment<TValue>? ChildContent { get; set; }
}