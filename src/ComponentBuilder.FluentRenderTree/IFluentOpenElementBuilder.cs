namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Provides constructors for creating HTML elements.
/// </summary>
public interface IFluentOpenElementBuilder : IDisposable
{
    /// <summary>
    /// Creates the opening tag of an HTML element.
    /// </summary>
    /// <param name="name">The name of the HTML element.</param>
    /// <param name="sequence">A sequence representing source code.</param>
    IFluentAttributeBuilder Element(string name, int? sequence = default);
}
