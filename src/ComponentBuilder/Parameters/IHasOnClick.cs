namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides click event for component.
/// </summary>
public interface IHasOnClick:IBlazorComponent
{
    /// <summary>
    /// Performed a callback when component is clicked.
    /// </summary>
    [HtmlEvent("onclick")] EventCallback<MouseEventArgs> OnClick { get; set; }
}
