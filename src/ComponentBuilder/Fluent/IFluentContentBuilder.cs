namespace ComponentBuilder.Fluent;

/// <summary>
/// Provides fragment of content builder.
/// </summary>
public interface IFluentContentBuilder :IFluentOpenBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// Add fragment content to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="fragment">The fragment of content to insert into inner element.</param>
    /// <returns>A <see cref="IFluentContentBuilder"/> instance contains inner content.</returns>
    IFluentContentBuilder Content(RenderFragment? fragment);
}
