using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// 表示 DOM 节点。
/// </summary>
public abstract class DomNode
{
    /// <summary>
    /// 初始化 <see cref="DomNode"/> 类的新实例。
    /// </summary>
    /// <param name="customizeModule">自定义模块。</param>
    /// <param name="internalModule">内部函数模块。</param>
    protected DomNode(IJSObjectReference? customizeModule = default, IJSObjectReference? internalModule = default)
    {
        CustomizeModule = customizeModule;
        InternalModule = internalModule;
    }
    /// <summary>
    /// 获取自定义的 JS 函数模块。
    /// </summary>
    public IJSObjectReference? CustomizeModule { get; }
    /// <summary>
    /// 获取内置的 JS 函数模块。
    /// </summary>
    public IJSObjectReference? InternalModule { get; }
}
