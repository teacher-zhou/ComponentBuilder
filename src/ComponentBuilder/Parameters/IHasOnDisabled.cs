namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备可禁用的事件。
/// </summary>
public interface IHasOnDisabled : IHasDisabled, IRefreshableComponent
{
    /// <summary>
    /// 设置当组件被禁用的回调函数。
    /// </summary>
    EventCallback<bool> OnDisabled { get; set; }
}
