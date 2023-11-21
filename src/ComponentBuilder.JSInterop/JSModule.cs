using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// Represents the exported JS module object.
/// </summary>
public interface IJSModule
{
    /// <summary>
    /// Gets <see cref="Window"/> instance.
    /// </summary>
    public Window Window { get; }
    /// <summary>
    /// Gets the exported module.
    /// </summary>
    public IJSObjectReference Module { get; }
}

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
