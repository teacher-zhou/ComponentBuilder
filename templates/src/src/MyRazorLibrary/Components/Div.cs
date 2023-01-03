namespace MyRazorLibrary;

/// <summary>
/// Represents a div HTML element.
/// </summary>
[HtmlTag("div")]
public class Div : BlazorComponentBase, IHasChildContent, IHasAdditionalClass, IHasAdditionalStyle
{
    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }
    /// <inheritdoc/>
    [Parameter]public string? AdditionalStyle { get; set; }
    /// <inheritdoc/>
    [Parameter]public string? AdditionalClass { get; set; }
}
