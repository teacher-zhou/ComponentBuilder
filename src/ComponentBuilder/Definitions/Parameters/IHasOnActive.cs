namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides a parameter that the component has a callback event that can be activated.
/// </summary>
public interface IHasOnActive : IHasActive, IHasEventCallback
{
    /// <summary>
    /// A callback function that is executed when the component state is activated.
    /// </summary>
    EventCallback<bool> OnActive { get; set; }
}
