using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;
/// <summary>
/// The extensions of <see cref="IJSRuntime"/> instance.
/// </summary>
public static class JSInteropExtensions
{
    /// <summary>
    /// 导入指定的 JS 模块。
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="content">
    /// 要导入的 JS 模块的内容。
    /// </param>
    public static async ValueTask<IJSModule> ImportAsync(this IJSRuntime js, string content)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", content);
        var window = await js.GetWindowAsync();
        return new JSModule(window, module);
    }

    /// <summary>
    /// 获取 <see cref="Window"/> 实例。
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    public static ValueTask<Window> GetWindowAsync(this IJSRuntime js) => ValueTask.FromResult(new Window(js));

    /// <summary>
    /// 在运行时异步计算指定的 javascript 字符串。
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="javascript">要执行的 javascript 代码。</param>
    public static ValueTask EvaluateAsync(this IJSRuntime js, string javascript) => js.InvokeVoidAsync("eval", javascript);

    /// <summary>
    /// 确定当前的托管环境是否支持 WebAssembly。
    /// </summary>
    /// <value>如果支持 WebAssembly 则返回 <c>true</c>；否则返回 <c>false</c>。</value>
    public static bool IsWebAssembly(this IJSRuntime js) => js is IJSInProcessRuntime;
}