namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides event parameters that a component can click on.
/// </summary>
public interface IHasOnClick : IBlazorComponent
{
    /// <summary>
    /// A callback function to be executed when the component is clicked, passing in event parameters.
    /// </summary>
    [HtmlEvent("onclick")] EventCallback<MouseEventArgs?> OnClick { get; set; }
}