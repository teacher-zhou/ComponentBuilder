namespace ComponentBuilder.Parameters;
public interface IHasOnSwitch : IHasSwitch, IRefreshComponent
{
    EventCallback<int?> OnSwitch { get; set; }
}
