using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// 表示被导出的 JS 模块对象。
/// </summary>
public interface IJSModule
{
    /// <summary>
    /// 获取 window 对象。
    /// </summary>
    public Window Window { get; }
    /// <summary>
    /// 获取被导出的模块。
    /// </summary>
    public IJSObjectReference Module { get; }
}

/// <summary>
/// 对 <see cref="IJSModule"/> 的基本实现。
/// </summary>
internal class JSModule : IJSModule
{
    internal JSModule(Window js, IJSObjectReference module)
    {
        Window = js;
        Module = module;
    }

    /// <inheritdoc/>
    public Window Window { get; }
    /// <inheritdoc/>
    public IJSObjectReference Module { get; }
}
