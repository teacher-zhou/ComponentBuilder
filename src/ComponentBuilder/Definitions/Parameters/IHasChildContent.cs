namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides UI fragment for component to support child content.
/// <para>
/// Implement this interface can create fragment content automatically.
/// </para>
/// </summary>
public interface IHasChildContent
{
    /// <summary>
    /// Gets or sets the fragment of UI content.
    /// </summary>
    RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// Provides UI fragment for component to support child content.
/// </summary>
/// <typeparam name="TValue">The type of object.</typeparam>
public interface IHasChildContent<TValue>
{
    /// <summary>
    /// Gets or sets the fragment of UI content with <typeparamref name="TValue"/>.
    /// </summary>
    RenderFragment<TValue>? ChildContent { get; set; }
}