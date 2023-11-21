using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// DOM node.
/// </summary>
public abstract class DomNode
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomNode"/> class.
    /// </summary>
    /// <param name="customizeModule">The customize module.</param>
    /// <param name="internalModule">The internal module.</param>
    protected DomNode(IJSObjectReference? customizeModule = default, IJSObjectReference? internalModule = default)
    {
        CustomizeModule = customizeModule;
        InternalModule = internalModule;
    }
    /// <summary>
    /// Get custom JS function modules.
    /// </summary>
    public IJSObjectReference? CustomizeModule { get; }
    /// <summary>
    /// Get internal JS function modules.
    /// </summary>
    public IJSObjectReference? InternalModule { get; }
}
