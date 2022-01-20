namespace ComponentBuilder.Parameters;
/// <summary>
/// Defines a callback event for <see cref="IHasActive"/> parameter.
/// </summary>
public interface IHasOnActive : IHasActive, IRefreshComponent
{
    /// <summary>
    /// Perform an action to active component state.
    /// </summary>
    EventCallback<bool> OnActive { get; set; }
}
