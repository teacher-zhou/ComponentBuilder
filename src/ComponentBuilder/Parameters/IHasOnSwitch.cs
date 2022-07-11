namespace ComponentBuilder.Parameters;
/// <summary>
/// Provides a function when specify index of components has switched.
/// </summary>
public interface IHasOnSwitch : IHasSwitch, IRefreshableComponent
{
    /// <summary>
    /// Perform an action when component collection is switched to with specify index.
    /// </summary>
    EventCallback<int?> OnSwitch { get; set; }
}
