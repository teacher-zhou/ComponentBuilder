using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// Represents a javascript module that exported.
/// </summary>
public interface IJSModule
{
    /// <summary>
    /// The window object in javascript.
    /// </summary>
    public Window Window { get; }
    /// <summary>
    /// The javascript module that is exported.
    /// </summary>
    public IJSObjectReference Module { get; }
}

/// <summary>
/// A default instance implemented from <see cref="IJSModule"/>.
/// </summary>
internal class JSModule:IJSModule
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
