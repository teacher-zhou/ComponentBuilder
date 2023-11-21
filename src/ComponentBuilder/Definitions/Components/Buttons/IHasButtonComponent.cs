namespace ComponentBuilder.Definitions;

/// <summary>
/// Defines a button component with &lt;button> HTML tag.
/// </summary>
[HtmlTag("button")]
public interface IHasButtonComponent : IBlazorComponent, IHasDisabled, IHasChildContent, IHasActive
{
    /// <summary>
    /// HTML type of button. Default is <see cref="ButtonHtmlType.Button"/>.
    /// </summary>
    [HtmlAttribute("type")] public ButtonHtmlType HtmlType { get; set; }
    /// <summary>
    /// Performs a callback action when clicking the button.
    /// </summary>
    [HtmlAttribute("onclick")] EventCallback<MouseEventArgs> OnClick { get; set; }
}
