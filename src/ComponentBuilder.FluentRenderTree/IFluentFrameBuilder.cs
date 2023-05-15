namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Provides a frame of render tree feature.
/// </summary>
public interface IFluentFrameBuilder: IFluentContentBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// Assigns the specified key value to the current element or component.
    /// </summary>
    /// <param name="value">The value for the key.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Key(object? value);

    /// <summary>
    /// Captures the reference for element.
    /// </summary>
    /// <param name="captureReferenceAction">An action to capture the reference of element after component is rendered.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Ref(Action<object?> captureReferenceAction);
}
