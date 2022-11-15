using Microsoft.JSInterop;

namespace ComponentBuilder;
/// <summary>
/// The extensions of <see cref="IJSRuntime"/> instance.
/// </summary>
public static class JSRuntimeExtensions
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
}
