namespace ComponentBuilder.Definitions;
/// <summary>
/// 提供可生成 &lt;a> 元素的超链接组件。
/// </summary>
[HtmlTag("a")]
public interface IAnchorComponent
{
    /// <summary>
    /// 访问链接。
    /// </summary>
    [HtmlAttribute] public string? Href { get; set; }
    /// <summary>
    /// 指定打开链接文档的位置。
    /// </summary>
    [HtmlAttribute] public AnchorTarget Target { get; set; }
}

/// <summary>
/// 锚(&lt;a>)元素的目标。
/// </summary>
public enum AnchorTarget
{
    /// <summary>
    /// 这是默认值。它打开同一框架中的链接文档。
    /// </summary>
    [HtmlAttribute(Value = "_self")] Self = 0,
    /// <summary>
    /// 它会在一个新窗口中打开链接。
    /// </summary>
    [HtmlAttribute(Value = "_blank")] Blank = 1,
    /// <summary>
    /// 它打开父框架集中的链接文档。
    /// </summary>
    [HtmlAttribute(Value = "_parant")] Parent = 2,
    /// <summary>
    /// 它在窗口的整个主体中打开链接的文档。
    /// </summary>
    [HtmlAttribute(Value = "_top")] Top = 3
}