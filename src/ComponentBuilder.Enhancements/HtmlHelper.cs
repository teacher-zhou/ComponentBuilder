namespace ComponentBuilder;

/// <summary>
/// HTML 的辅助。
/// </summary>
public sealed class HtmlHelper
{
    private HtmlHelper() { }

    /// <summary>
    /// 获取当前实例。
    /// </summary>
    public static HtmlHelper Instance => new();
    /// <summary>
    /// 合并 HTML 属性并替换相同名称的值。
    /// </summary>
    /// <param name="htmlAttributes">
    /// 要合并的 HTML 属性或组件参数。
    /// <para>
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <returns>包含 HTML 属性的键值对的集合。</returns>
    public IEnumerable<KeyValuePair<string, object>>? MergeHtmlAttributes(object htmlAttributes)
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
    /// 通过给定的操作创建 HTML 属性。
    /// </summary>
    /// <param name="htmlAttributes">创建 HTML 属性的方法。</param>
    /// <returns>包含 HTML 属性的键值对的集合。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="htmlAttributes"/> 是 <c>null</c>。</exception>
    public IEnumerable<KeyValuePair<string, object>>? CreateHtmlAttributes(Action<IDictionary<string, object>> htmlAttributes)
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
    /// 创建指定标记文本内容的UI呈现片段。
    /// </summary>
    /// <param name="markupContent">要呈现的标记文本内容。</param>
    /// <returns>可渲染的UI片段。</returns>
    public RenderFragment? CreateContent(MarkupString? markupContent)
        => builder => builder.AddContent(0, markupContent);

    /// <summary>
    /// 创建指定文本内容的UI呈现片段。
    /// </summary>
    /// <param name="textContent">要呈现的文本内容。</param>
    /// <returns>可渲染的UI片段。</returns>
    public RenderFragment? CreateContent(string? textContent)
        => builder => builder.AddContent(0, textContent);

    /// <summary>
    /// 创建指定任意内容的UI渲染片段。
    /// </summary>
    /// <param name="fragment">要呈现的任意内容。</param>
    /// <returns>可渲染的UI片段。</returns>
    public RenderFragment? CreateContent(RenderFragment? fragment)
        => builder => builder.AddContent(0, fragment);

    /// <summary>
    /// 获取 <see cref="EventCallbackFactory"/> 实例。
    /// </summary>
    public EventCallbackFactory Callback() => EventCallback.Factory;
}
