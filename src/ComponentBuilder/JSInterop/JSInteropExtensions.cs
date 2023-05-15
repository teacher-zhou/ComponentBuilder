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
    /// <param name="path">
    /// The path of javascript file to import. Such as <c>./js/app.js</c> in wwwroot path.
    /// </param>
    /// <returns>A <see cref="ValueTask{TResult}"/> containing dynamic reference object of javascript.</returns>
    public static async ValueTask<dynamic> ImportAsync(this IJSRuntime js, string path)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", path);
        return new DynamicJsReferenceObject(module);
    }

    /// <summary>
    /// Asynchronously evaluate a specified javascript string in runtime.
    /// </summary>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="javascript">The javascript content to evaluate.</param>
    /// <returns>A <see cref="ValueTask"/> represents javascript string evaluation.</returns>
    public static ValueTask EvaluateAsync(this IJSRuntime js, string javascript) => js.InvokeVoidAsync("eval", javascript);

    /// <summary>
    /// Asynchrousely evaluate a specified javascript in runtime.
    /// </summary>
    /// <remarks>The method does not stable and is limitation.</remarks>
    /// <param name="js">Instance of <see cref="IJSRuntime"/>.</param>
    /// <param name="javascript">A action represents javascript code.</param>
    /// <returns>A <see cref="ValueTask"/> represents javascript string evaluation.</returns>
    public static ValueTask EvaluateAsync(this IJSRuntime js, Action<dynamic> javascript)
    {
        using var script = new ScriptBuilder();
        javascript(script);
        var scriptValue = script.ToString();
        return js.EvaluateAsync(scriptValue);
    }

    /// <summary>
    /// Determines hosting environment is WebAssembly.
    /// </summary>
    /// <value><c>True</c> to WebAssembly, otherwise, <c>false</c>.</value>
    public static bool IsWebAssembly(this IJSRuntime js) => js is IJSInProcessRuntime;
}