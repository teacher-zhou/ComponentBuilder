namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Provides constructor functionality for creating components.
/// </summary>
public interface IFluentOpenComponentBuilder : IDisposable
{
    /// <summary>
    /// Creates a component start tag that implements the <see cref="IComponent"/> interface component type.
    /// </summary>
    /// <param name="componentType">Component type to create.</param>
    /// <param name="sequence">A sequence representing source code.</param>
    IFluentAttributeBuilder Component(Type componentType, int? sequence = default);

    /// <summary>
    /// Add parameters to the component.
    /// </summary>
    /// <param name="name">Parameter name.</param>
    /// <param name="value">Parameter value.</param>
    /// <returns></returns>
    IFluentAttributeBuilder Parameter(string name,object? value);
}

/// <summary>
/// Provides constructor functionality for creating components.
/// </summary>
/// <typeparam name="TComponent">Component type.</typeparam>
public interface IFluentOpenComponentBuilder<TComponent> : IFluentOpenComponentBuilder
    where TComponent : IComponent
{
    /// <summary>
    /// Creates a component start tag that implements the <see cref="IComponent"/> interface component type.
    /// </summary>
    /// <param name="sequence">A sequence representing source code.</param>
    IFluentAttributeBuilder<TComponent> Component(int? sequence = default);
}