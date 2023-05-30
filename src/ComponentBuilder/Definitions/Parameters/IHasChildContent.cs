namespace ComponentBuilder.Definitions;

/// <summary>
/// 为组件提供 UI 片段以支持子内容。
/// <para>
/// 实现该接口可以自动创建片段内容。
/// </para>
/// </summary>
public interface IHasChildContent
{
    /// <summary>
    /// 获取或设置 UI 内容的片段。
    /// </summary>
    RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// 为组件提供 UI 片段以支持子内容。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
public interface IHasChildContent<TValue>
{
    /// <summary>
    ///获取或设置带有 <typeparamref name="TValue"/> 的 UI 内容片段。
    /// </summary>
    RenderFragment<TValue>? ChildContent { get; set; }
}