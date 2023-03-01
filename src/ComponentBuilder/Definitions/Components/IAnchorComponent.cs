namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides a &lt;a> tag element of component to create hyperlink.
/// </summary>
[HtmlTag("a")]
public interface IAnchorComponent : IBlazorComponent
{
    /// <summary>
    /// The link of visit.
    /// </summary>
    [HtmlAttribute] public string? Href { get; set; }
    /// <summary>
    /// Specifies where to open the linked document.
    /// </summary>
    [HtmlAttribute] public AnchorTarget Target { get; set; }
}

/// <summary>
/// The targets of anchor(&lt;a>) element.
/// </summary>
public enum AnchorTarget
{
    /// <summary>
    /// It is the default value. It opens the linked document in the same frame.
    /// </summary>
    [HtmlAttribute(Value = "_self")] Self = 0,
    /// <summary>
    /// It opens the link in a new window.
    /// </summary>
    [HtmlAttribute(Value = "_blank")] Blank = 1,
    /// <summary>
    /// It opens the linked document in the parent frameset.
    /// </summary>
    [HtmlAttribute(Value = "_parant")] Parent = 2,
    /// <summary>
    /// It opens the linked document in the full body of the window.
    /// </summary>
    [HtmlAttribute(Value = "_top")] Top = 3
}