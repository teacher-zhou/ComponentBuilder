namespace ComponentBuilder.Parameters;
/// <summary>
/// 提供组件具备可激活的回调事件的参数。
/// </summary>
public interface IHasOnActive : IHasActive, IRefreshableComponent
{
    /// <summary>
    /// 设置一个组件状态被激活执行的回调函数。
    /// </summary>
    EventCallback<bool> OnActive { get; set; }
}
