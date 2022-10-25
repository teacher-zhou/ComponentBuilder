namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides event parameters that a component can click on.
/// </summary>
public interface IHasOnClick : IHasOnClick<MouseEventArgs?>
{
}

/// <summary>
/// Provides event parameters that a component can click on.
/// </summary>
/// <typeparam name="TEventArgs">The type of event arguments.</typeparam>
public interface IHasOnClick<TEventArgs> : IRefreshableComponent
{
    /// <summary>
    /// A callback function to be executed when the component is clicked, passing in event parameters.
    /// </summary>
    [HtmlEvent("onclick")] EventCallback<TEventArgs?> OnClick { get; set; }
}