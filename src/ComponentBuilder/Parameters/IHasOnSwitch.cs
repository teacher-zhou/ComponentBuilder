namespace ComponentBuilder.Parameters;
public interface IHasOnSwitch : IHasSwitch, IRefreshableComponent
{
    EventCallback<int?> OnSwitch { get; set; }
}
