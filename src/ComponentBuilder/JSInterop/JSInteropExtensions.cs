using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;
/// <summary>
/// The extensions of <see cref="IJSRuntime"/> instance.
/// </summary>
public static class JSInteropExtensions
{
    /// <summary>
    /// Import the specified JS module.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="content">
    /// The contents of the JS module to be imported.
    /// </param>
    public static async ValueTask<IJSModule> ImportAsync(this IJSRuntime js, string content)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", content);
        var window = await js.GetWindowAsync();
        return new JSModule(window, module);
    }

    /// <summary>
    /// Get <see cref="Window"/> instance.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    public static ValueTask<Window> GetWindowAsync(this IJSRuntime js) => ValueTask.FromResult(new Window(js));

    /// <summary>
    /// Evaluates the specified javascript string asynchronously at run time.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="javascript">javascript code to execute.</param>
    public static ValueTask EvaluateAsync(this IJSRuntime js, string javascript) => js.InvokeVoidAsync("eval", javascript);

    /// <summary>
    /// Determine if your current hosting environment supports WebAssembly.
    /// </summary>
    /// <value>Return <c>true</c> if WebAssembly is supported; Otherwise, return <c>false</c>.</value>
    public static bool IsWebAssembly(this IJSRuntime js) => js is IJSInProcessRuntime;
}