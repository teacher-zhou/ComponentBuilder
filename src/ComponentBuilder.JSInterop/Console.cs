
using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

public class Console : Interop
{
    internal Console(IJSRuntime globalJS, IJSObjectReference? customizeModule = null, IJSObjectReference? internalModule = null) : base(globalJS, customizeModule, internalModule)
    {
    }

    /// <summary>
    /// Outputs a message to the console at the "debug" log level.
    /// </summary>
    /// <param name="args">A list of JavaScript objects to output. The string representations of each of these objects are appended together in the order listed and output to the console.</param>
    public ValueTask DebugAsync(params object?[] args) => GlobalJS.InvokeVoidAsync("console.debug", args);

    /// <summary>
    /// Outputs a message to the console at the "error" log level.
    /// </summary>
    /// <param name="args">A list of JavaScript objects to output. The string representations of each of these objects are appended together in the order listed and output to the console.</param>
    public ValueTask ErrorAsync(params object?[] args) => GlobalJS.InvokeVoidAsync("console.error", args);

    /// <summary>
    /// Outputs a message to the console at the "trace" log level.
    /// </summary>
    /// <param name="args">A list of JavaScript objects to output. The string representations of each of these objects are appended together in the order listed and output to the console.</param>
    public ValueTask TraceAsync(params object?[] args) => GlobalJS.InvokeVoidAsync("console.trace", args);

    /// <summary>
    /// Outputs a message to the console at the "info" log level.
    /// </summary>
    /// <param name="args">A list of JavaScript objects to output. The string representations of each of these objects are appended together in the order listed and output to the console.</param>
    public ValueTask InfoAsync(params object?[] args) => GlobalJS.InvokeVoidAsync("console.info", args);


    /// <summary>
    /// Outputs a message to the console.
    /// </summary>
    /// <param name="args">A list of JavaScript objects to output. The string representations of each of these objects are appended together in the order listed and output to the console.</param>
    public ValueTask LogAsync(params object?[] args) => GlobalJS.InvokeVoidAsync("console.log", args);
    /// <summary>
    ///  clears the console if the console allows it.
    /// </summary>
    public ValueTask ClearAsync() => GlobalJS.InvokeVoidAsync("clear");
}
