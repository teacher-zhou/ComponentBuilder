namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Constructors that provide additional features of the render tree.
/// </summary>
public interface IFluentFrameBuilder: IFluentContentBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// Assigns the specified key value to the current element or component.
    /// </summary>
    /// <param name="value">The key value.</param>
    IFluentAttributeBuilder Key(object? value);

    /// <summary>
    /// Captures a reference to an element or component.
    /// </summary>
    /// <param name="captureReferenceAction">Actions that capture an element or component reference after rendering the component.</param>
    IFluentAttributeBuilder Ref(Action<object?> captureReferenceAction);

    /// <summary>
    /// Adds a frame to indicate the rendering mode on the closed component frame.
    /// </summary>
    /// <param name="mode"><see cref="IComponentRenderMode"/> mode.</param>
    IFluentAttributeBuilder RenderMode(IComponentRenderMode mode);
}
