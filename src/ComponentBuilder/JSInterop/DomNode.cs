using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// DOM node.
/// </summary>
public abstract class Interop:IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Interop"/> class.
    /// </summary>
    /// <param name="globalJS">The global JS object.</param>
    /// <param name="customizeModule">The customize module.</param>
    /// <param name="internalModule">The internal module.</param>
    protected Interop(IJSRuntime globalJS, IJSObjectReference? customizeModule = default, IJSObjectReference? internalModule = default)
    {
        GlobalJS = globalJS;
        CustomizeModule = customizeModule;
        InternalModule = internalModule;
    }

    /// <summary>
    /// Gets the global JS object.
    /// </summary>
    protected IJSRuntime GlobalJS { get; }

    /// <summary>
    /// Get custom JS function modules.
    /// </summary>
    protected IJSObjectReference? CustomizeModule { get; }
    /// <summary>
    /// Get internal JS function modules.
    /// </summary>
    protected IJSObjectReference? InternalModule { get; }
/// <inheritdoc/>

    public virtual ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
