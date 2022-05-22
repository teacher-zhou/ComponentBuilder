namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件可包含任务 UI 片段内容的参数。
/// </summary>
public interface IHasChildContent
{
    /// <summary>
    /// 可渲染的 UI 片段。
    /// </summary>
    RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// 提供组件可包含任务 UI 片段内容的参数。
/// </summary>
/// <typeparam name="TValue">The type of object.</typeparam>
public interface IHasChildContent<TValue>
{
    /// <summary>
    /// 可渲染 <typeparamref name="TValue"/> 值得 UI 片段内容。
    /// </summary>
    RenderFragment<TValue>? ChildContent { get; set; }
}