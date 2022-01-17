namespace ComponentBuilder.Parameters;

/// <summary>
/// Defines a callback event for <see cref="IHasDisabled"/> parameter.
/// </summary>
public interface IHasOnDisabled:IHasDisabled,IBlazorComponent
{
    /// <summary>
    /// Perform an action to disable component state.
    /// </summary>
    EventCallback<bool> OnDisabled { get; set; }
}
