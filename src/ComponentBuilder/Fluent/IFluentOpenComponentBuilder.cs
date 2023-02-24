namespace ComponentBuilder.Fluent;

/// <summary>
/// Provides a frame to create component.
/// </summary>
public interface IFluentOpenComponentBuilder : IDisposable
{
    /// <summary>
    /// Create an open component of frame with specified component type.
    /// </summary>
    /// <param name="componentType">The type of component.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Component(Type componentType);

    /// <summary>
    /// Create an open component of frame with specified component type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Component<TComponent>() where TComponent : IComponent
        => Component(typeof(TComponent));
}
