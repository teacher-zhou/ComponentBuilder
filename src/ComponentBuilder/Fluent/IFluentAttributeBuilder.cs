namespace ComponentBuilder.Fluent;
/// <summary>
/// Provides attributes builder.
/// </summary>
public interface IFluentAttributeBuilder : IFluentContentBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// Add element attribute or component parameter and attribute.
    /// </summary>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null or empty string.</exception>
    IFluentAttributeBuilder Attribute(string name, object? value);
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
