namespace ComponentBuilder.Definitions;
/// <summary>
/// Provide generative &lt; a> Hyperlinked component of the element.
/// </summary>
[HtmlTag("a")]
public interface IAnchorComponent
{
    /// <summary>
    /// The href link of anchor.
    /// </summary>
    [HtmlAttribute] public string? Href { get; set; }
    /// <summary>
    /// Specify where to open the linked document.
    /// </summary>
    [HtmlAttribute] public AnchorTarget Target { get; set; }
}

/// <summary>
/// Anchor (&lt; a>) target of the element.
/// </summary>
public enum AnchorTarget
{
    /// <summary>
    /// This is the default value. It opens linked documents in the same frame.
    /// </summary>
    [HtmlAttribute(value: "_self")] Self = 0,
    /// <summary>
    /// It opens the link in a new window.
    /// </summary>
    [HtmlAttribute(value: "_blank")] Blank = 1,
    /// <summary>
    /// It opens the linked document in the parent frame set.
    /// </summary>
    [HtmlAttribute(value: "_parant")] Parent = 2,
    /// <summary>
    /// It opens the linked document in the entire body of the window.
    /// </summary>
    [HtmlAttribute(value: "_top")] Top = 3
}