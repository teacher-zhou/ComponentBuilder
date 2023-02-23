namespace ComponentBuilder.Fluent;
/// <summary>
/// Provides an open frame for element or component.
/// </summary>
public interface IFluentOpenBuilder : IFluentOpenElementBuilder,IFluentOpenComponentBuilder
{
    /// <summary>
    /// Add fragment content to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="fragment">The fragment of content to insert into inner element.</param>
    /// <returns>A <see cref="IFluentOpenBuilder"/> instance contains inner content.</returns>
    IFluentOpenBuilder Content(RenderFragment? fragment);
}
