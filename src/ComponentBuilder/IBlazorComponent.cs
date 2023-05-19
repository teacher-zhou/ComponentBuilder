namespace ComponentBuilder;

/// <summary>
/// 表示一个 Blazor 组件。
/// </summary>
public interface IBlazorComponent : IHasAdditionalAttributes, IComponent, IDisposable
{
    /// <summary>
    /// 获取 HTML 元素实例的引用。
    /// </summary>
    ElementReference? Reference { get; }

    /// <summary>
    /// 获取 <see cref="ICssClassBuilder"/> 实例。
    /// </summary>
    ICssClassBuilder CssClassBuilder { get; }

    /// <summary>
    /// 获取 <see cref="IStyleBuilder"/> 实例。
    /// </summary>
    IStyleBuilder StyleBuilder { get; }
    /// <summary>
    /// 获取子组件集合。
    /// </summary>
    BlazorComponentCollection ChildComponents { get; }

    /// <summary>
    /// 异步地通知组件其状态已更改并呈现该组件。
    /// </summary>
    Task NotifyStateChanged();

    /// <summary>
    /// 获取每个项目用空格分隔的类字符串。
    /// </summary>
    /// <returns>由空格分隔的字符串，每个条目或<c>null</c>。</returns>
    public string? GetCssClassString();
    /// <summary>
    /// 获取每个项目用分号(;)分隔的样式字符串。
    /// </summary>
    /// <returns>用分号(;)分隔的字符串，每个项或<c>null</c>。 </returns>
    public string? GetStyleString();

    /// <summary>
    /// 返回 HTML 元素名称。
    /// </summary>
    /// <returns>字符串表示HTML元素标签名称。</returns>
    /// <exception cref="ArgumentException">值是 <c>null</c> 或空字符串。</exception>
    string GetTagName();

    /// <summary>
    /// 构建具有自动特性的组件。
    /// </summary>
    /// <param name="builder">渲染树。</param>
    void BuildComponent(RenderTreeBuilder builder);

    /// <summary>
    /// 返回组件的属性。
    /// </summary>
    /// <returns>包含 HTML 属性的键/值对。</returns>
    IEnumerable<KeyValuePair<string, object>> GetAttributes();
}
