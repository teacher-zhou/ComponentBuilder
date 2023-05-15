namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Provides a frame to create element.
/// </summary>
public interface IFluentOpenElementBuilder : IDisposable
{
    /// <summary>
    /// Create an open element of frame.
    /// </summary>
    /// <param name="name">The name of element.</param>
    /// <param name="sequence">A sequence representing the position of source code.</param>
    /// <returns>The <see cref="IFluentAttributeBuilder"/> instance.</returns>
    IFluentAttributeBuilder Element(string name, int? sequence = default);
}
