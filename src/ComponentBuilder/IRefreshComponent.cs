namespace ComponentBuilder;

/// <summary>
/// 为组件提供可在状态更改后进行刷新的功能。
/// </summary>
public interface IRefreshableComponent
{
    /// <summary>
    /// 通知组件状态已更改并重新呈现组件。
    /// </summary>
    /// <returns>一个任务操作，任务返回后不包含返回值。</returns>
    Task NotifyStateChanged();
}
