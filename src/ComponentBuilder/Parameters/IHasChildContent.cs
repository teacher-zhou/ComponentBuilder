namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides a component has child content parameter.
/// </summary>
public interface IHasChildContent
{
    /// <summary>
    /// A segment of UI content to render.
    /// </summary>
    RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// Provides a component has child content parameter.
/// </summary>
/// <typeparam name="TValue">The type of object.</typeparam>
public interface IHasChildContent<TValue>
{
    /// <summary>
    /// A segment of UI content to render for an object of <typeparamref name="TValue"/>..
    /// </summary>
    RenderFragment<TValue>? ChildContent { get; set; }
}