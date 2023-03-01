namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides event parameters that a component can click on.
/// </summary>
public interface IHasOnClick : IBlazorComponent
{
    /// <summary>
    /// A callback function to be executed when the component is clicked, passing in event parameters.
    /// </summary>
    [HtmlAttribute("onclick")] EventCallback<MouseEventArgs?> OnClick { get; set; }
}