


namespace ComponentBuilder.Definitions;

/// <summary>
/// Defines a button component with basic parameters.
/// </summary>
[HtmlTag("button")]
public abstract class ButtonComponentBase : BlazorComponentBase, IHasButtonComponent
{
    /// <inheritdoc/>
    [Parameter]public bool Disabled { get; set; }

    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// HTML type of button. Default is <see cref="ButtonHtmlType.Button"/>.
    /// </summary>
    [Parameter][HtmlAttribute("type")] public ButtonHtmlType HtmlType { get; set; } = ButtonHtmlType.Button;
    /// <inheritdoc/>
    [Parameter]public EventCallback<MouseEventArgs> OnClick { get; set; }
    /// <inheritdoc/>
    [Parameter]public bool Active { get; set; }
}

