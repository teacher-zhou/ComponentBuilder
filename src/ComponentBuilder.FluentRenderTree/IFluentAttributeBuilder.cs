using System.Linq.Expressions;

namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// A constructor that provides attributes for building elements or parameters for components.
/// </summary>
public interface IFluentAttributeBuilder : IFluentFrameBuilder
{
    /// <summary>
    /// Add element attributes or component parameters and attributes.
    /// </summary>
    /// <param name="name">HTML property name or parameter name of the component.</param>
    /// <param name="value">The value of a property or parameter.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <c>null</c> or empty.</exception>
    IFluentAttributeBuilder Attribute(string name, object? value);
}

/// <summary>
/// A constructor that provides parameters for building components.
/// </summary>
/// <typeparam name="TComponent">The compnent type.</typeparam>
public interface IFluentAttributeBuilder<TComponent> : IFluentAttributeBuilder, IFluentContentBuilder<TComponent>,IFluentCloseBuilder<TComponent>
    where TComponent : IComponent
{
    /// <summary>
    /// Add parameters and values for the component.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="parameter">Parameter selector.</param>
    /// <param name="value">Parameter value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameter"/> is <c>null</c>.</exception>
    IFluentAttributeBuilder<TComponent> Attribute<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue? value);
}