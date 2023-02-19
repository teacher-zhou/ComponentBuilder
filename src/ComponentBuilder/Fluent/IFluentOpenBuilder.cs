namespace ComponentBuilder.Fluent;
/// <summary>
/// Provides an open frame for element or component.
/// </summary>
public interface IFluentOpenBuilder : IDisposable
{
    /// <summary>
    /// Represents an open element of frame.
    /// </summary>
    /// <param name="name">The name of element.</param>
    /// <param name="sequence">An integer represents the position of source code in frames.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Element(string name, int sequence = 0);

    /// <summary>
    /// Represents an open component of frame.
    /// </summary>
    /// <param name="componentType">The type of component.</param>
    /// <param name="sequence">An integer represents the position of source code in frames.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Component(Type componentType, int sequence = 0);
}
