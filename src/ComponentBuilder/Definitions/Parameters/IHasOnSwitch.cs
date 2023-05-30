namespace ComponentBuilder.Definitions;
/// <summary>
/// 提供具有可切换事件的组件。
/// </summary>
public interface IHasOnSwitch : IHasSwitch, IHasEventCallback
{
    /// <summary>
    /// 指定组件索引的可切换回调方法。
    /// </summary>
    EventCallback<int?> OnSwitch { get; set; }
}
