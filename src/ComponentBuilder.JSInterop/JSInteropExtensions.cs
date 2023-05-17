using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;
/// <summary>
/// The extensions of <see cref="IJSRuntime"/> instance.
/// </summary>
public static class JSInteropExtensions
{
    /// <summary>
    /// Asynchronously import specified javascript.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="content">
    /// The path of javascript file to import. Such as <c>./js/app.js</c> in wwwroot path.
    /// </param>
    /// <returns>A <see cref="ValueTask{TResult}"/> contains <see cref="IJSModule"/> object.</returns>
    public static async ValueTask<IJSModule> ImportAsync(this IJSRuntime js, string content)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", content);
        var window = await js.EvaluateWindowAsync();
        return new JSModule(window, module);
    }

    /// <summary>
    /// Asynchronously evaluate <see cref="Window"/> instance.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    public static ValueTask<Window> EvaluateWindowAsync(this IJSRuntime js) => ValueTask.FromResult(new Window(js));

    /// <summary>
    /// Asynchronously evaluate a specified javascript string in runtime.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="javascript">The javascript content to evaluate.</param>
    /// <returns>A <see cref="ValueTask"/> represents javascript string evaluation.</returns>
    public static ValueTask EvaluateAsync(this IJSRuntime js, string javascript) => js.InvokeVoidAsync("eval", javascript);

    /// <summary>
    /// Determines hosting environment is WebAssembly.
    /// </summary>
    /// <value><c>True</c> to WebAssembly, otherwise, <c>false</c>.</value>
    public static bool IsWebAssembly(this IJSRuntime js) => js is IJSInProcessRuntime;
}