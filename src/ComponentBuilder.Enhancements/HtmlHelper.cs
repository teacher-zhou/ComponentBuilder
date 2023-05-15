namespace ComponentBuilder.Enhancement;

/// <summary>
/// The helpers of HTML object.
/// </summary>
public class HtmlHelper
{
    /// <summary>
    /// Merge HTML attributes and replace value for same name.
    /// </summary>
    /// <param name="htmlAttributes">
    /// The HTML attributes or component parameters to merge.
    /// <para>
    /// Support anonymous class like <c>new { @class="class1", id="my-id" , onclick = xxx }</c>.
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
    /// Create HTML attributes by given action。
    /// </summary>
    /// <param name="htmlAttributes">Method that executes HTML attributes.</param>
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
    /// Creates a UI render fragment of the specified markup text content.
    /// </summary>
    /// <param name="markupContent">The markup text content to render.</param>
    /// <returns>Renderable UI fragments.</returns>
    public static RenderFragment? CreateContent(MarkupString? markupContent)
        => builder => builder.AddContent(0, markupContent);

    /// <summary>
    /// Creates a UI render fragment of the specified text content.
    /// </summary>
    /// <param name="textContent">The text content to render.</param>
    /// <returns>Renderable UI fragments.</returns>
    public static RenderFragment? CreateContent(string? textContent)
        => builder => builder.AddContent(0, textContent);

    /// <summary>
    /// Creates a UI render fragment of the specified an arbitrary content.
    /// </summary>
    /// <param name="fragment">The arbitrary content to render.</param>
    /// <returns>Renderable UI fragments.</returns>
    public static RenderFragment? CreateContent(RenderFragment? fragment)
        => builder => builder.AddContent(0, fragment);

    /// <summary>
    /// Creates a callback for HTML event.
    /// </summary>
    public static EventCallbackFactory CreateCallback() => EventCallback.Factory;
}
