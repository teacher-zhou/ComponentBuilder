namespace MyRazorLibrary;

/// <summary>
/// Represents a div HTML element.
/// </summary>
[HtmlTag("div")]
public class Div : BlazorComponentBase, IHasChildContent
{
    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }
}
