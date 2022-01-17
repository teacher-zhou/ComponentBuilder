namespace ComponentBuilder.Parameters;
/// <summary>
/// Defines a callback event for <see cref="IHasActive"/> parameter.
/// </summary>
public interface IHasOnActived : IHasActive,IBlazorComponent
{
    /// <summary>
    /// Perform an action to active component state.
    /// </summary>
    EventCallback<bool> OnActived { get; set; }
}
