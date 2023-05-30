namespace ComponentBuilder.Definitions;

/// <summary>
/// 提供具有可禁用事件的组件。
/// </summary>
public interface IHasOnDisabled : IHasDisabled, IHasEventCallback
{
    /// <summary>
    /// 当组件被禁用时的回调函数。
    /// </summary>
    EventCallback<bool> OnDisabled { get; set; }
}
