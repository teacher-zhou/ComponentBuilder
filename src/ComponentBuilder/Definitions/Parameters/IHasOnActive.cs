namespace ComponentBuilder.Definitions;
/// <summary>
/// 提供组件具有可激活的回调事件的参数。
/// </summary>
public interface IHasOnActive : IHasActive, IHasEventCallback
{
    /// <summary>
    /// 当组件状态被激活时执行的回调函数。
    /// </summary>
    EventCallback<bool> OnActive { get; set; }
}
