

namespace ComponentBuilder.Components;

/// <summary>
/// Defines a button component with basic parameters.
/// </summary>
[HtmlTag("button")]
public abstract class ButtonComponentBase : BlazorComponentBase, IHasDisabled,IHasChildContent
{
    /// <inheritdoc/>
    [Parameter]public bool Disabled { get; set; }

    /// <inheritdoc/>
    [Parameter]public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// HTML type of button. Default is <see cref="ButtonHtmlType.Button"/>.
    /// </summary>
    [Parameter][HtmlAttribute("type")] public ButtonHtmlType HtmlType { get; set; } = ButtonHtmlType.Button;
}

/// <summary>
/// The HTML type of button.
/// </summary>
public enum ButtonHtmlType
{
    /// <summary>
    /// A normal button.
    /// </summary>
    Button,
    /// <summary>
    /// A submit button can trigger form onsubmit event.
    /// </summary>
    Submit,
    /// <summary>
    /// Reset and clear the value of input controls in form.
    /// </summary>
    Reset,
}