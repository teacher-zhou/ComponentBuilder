using Microsoft.JSInterop;

namespace ComponentBuilder.JSInterop;

/// <summary>
/// Represents a DOM node.
/// </summary>
public class DomNode
{
    /// <summary>
    /// Initializes a new instance of <see cref="DomNode"/> class.
    /// </summary>
    /// <param name="customizeModule"></param>
    /// <param name="internalModule"></param>
    protected DomNode(IJSObjectReference? customizeModule=default, IJSObjectReference? internalModule=default)
    {
        CustomizeModule = customizeModule;
        InternalModule = internalModule;
    }
    /// <summary>
    /// Gets the module exported by self customization.
    /// </summary>
    public IJSObjectReference? CustomizeModule { get; }
    /// <summary>
    /// Gets the module exported by internal javascript object.
    /// </summary>
    public IJSObjectReference? InternalModule { get; }
}
