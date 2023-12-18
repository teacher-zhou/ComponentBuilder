namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// Provides a constructor to create an element or component start tag.
/// </summary>
public interface IFluentOpenBuilder : IFluentOpenElementBuilder,IFluentOpenComponentBuilder,IFluentCloseBuilder
{
    /// <summary>
    /// Add fragment content.
    /// </summary>
    /// <param name="fragment">The content fragment to insert the inner element.</param>
    /// <param name="sequence">A sequence representing the location of the source code.</param>
    IFluentOpenBuilder Content(RenderFragment? fragment, int? sequence = default);
}
