using ComponentBuilder.Builder;

namespace ComponentBuilder;

/// <summary>
/// HTML helper。
/// </summary>
public static class HtmlHelper
{
    /// <summary>
    /// Creates <see cref="ICssClassBuilder"/> instance.
    /// </summary>
    public static ICssClassBuilder Class => new DefaultCssClassBuilder();

    /// <summary>
    /// Creates <see cref="IStyleBuilder"/> instance.
    /// </summary>
    public static IStyleBuilder Style => new DefaultStyleBuilder();

    /// <summary>
    /// Merge HTML attributes and replace values with the same name.
    /// </summary>
    /// <param name="htmlAttributes">
    /// HTML attributes or component parameters to merge.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <returns>A collection of key-value pairs containing HTML attributes.</returns>
    public static IEnumerable<KeyValuePair<string, object>>? MergeHtmlAttributes(object htmlAttributes)
        => htmlAttributes switch
        {
            IEnumerable<KeyValuePair<string, object>> dic => dic,
            object obj => obj.GetType().GetProperties()
                .Select(property =>
                {
                    var name = property.Name.Replace("_", "-");
                    var value = property.GetValue(htmlAttributes) ?? string.Empty;
                    return new KeyValuePair<string, object>(name, value);
                }).Distinct()
        };

    /// <summary>
    /// Create an HTML property with the given action.
    /// </summary>
    /// <param name="htmlAttributes">Methods for creating HTML attributes.</param>
    /// <returns>A collection of key-value pairs containing HTML attributes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="htmlAttributes"/> is <c>null</c>。</exception>
    public static IEnumerable<KeyValuePair<string, object>>? CreateHtmlAttributes(Action<IDictionary<string, object>> htmlAttributes)
    {
        if (htmlAttributes is null)
        {
            throw new ArgumentNullException(nameof(htmlAttributes));
        }

        var attributes = new Dictionary<string, object>();
        htmlAttributes(attributes);
        return attributes;
    }

    /// <summary>
    /// Creates a UI rendering fragment that specifies markup text content.
    /// </summary>
    /// <param name="markupContent">Markup text content to render.</param>
    /// <returns>Renderable UI fragments.</returns>
    public static RenderFragment? CreateContent(MarkupString? markupContent)
        => builder => builder.AddContent(0, markupContent);

    /// <summary>
    /// Creates a UI rendering fragment of the specified text content.
    /// </summary>
    /// <param name="textContent">Text content to render.</param>
    /// <returns>Renderable UI fragments.</returns>
    public static RenderFragment? CreateContent(string? textContent)
        => builder => builder.AddContent(0, textContent);

    /// <summary>
    /// Creates a UI render fragment that specifies any content.
    /// </summary>
    /// <param name="fragment">Any fregment content.</param>
    /// <returns>Renderable UI fragments.</returns>
    public static RenderFragment? CreateContent(RenderFragment? fragment)
        => builder => builder.AddContent(0, fragment);

    /// <summary>
    /// Gets <see cref="EventCallbackFactory"/> instance.
    /// </summary>
    public static EventCallbackFactory Callback => EventCallback.Factory;
}
