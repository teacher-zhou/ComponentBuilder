namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Provides a constructor for creating a new scope.
/// </summary>
public interface IFluentRegionBuilder:IDisposable
{
    /// <summary>
    /// Create a new scope.
    /// </summary>
    /// <param name="sequence">The starting range of a new source code location.</param>
    IFluentOpenBuilder Region(int sequence);
}