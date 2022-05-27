namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件可以被鼠标单击的事件参数。
/// </summary>
public interface IHasOnClick : IHasOnClick<MouseEventArgs>
{
}

/// <summary>
/// 提供组件可以被鼠标单击的事件参数。
/// </summary>
/// <typeparam name="TEventArgs">回调事件的类型。</typeparam>
public interface IHasOnClick<TEventArgs> : IRefreshComponent
{
    /// <summary>
    /// 设置当组件被单击时执行的回调函数，并传入事件参数。
    /// </summary>
    [HtmlEvent("onclick")] EventCallback<TEventArgs> OnClick { get; set; }
}