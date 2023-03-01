namespace ComponentBuilder.Fluent;

/// <summary>
/// Provides a frame to create element.
/// </summary>
public interface IFluentOpenElementBuilder : IDisposable
{
    /// <summary>
    /// Create an open element of frame.
    /// </summary>
    /// <param name="name">The name of element.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Element(string name);
}
