namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// Provides a frame to create a new region.
/// </summary>
public interface IFluentRegionBuilder:IDisposable
{
    /// <summary>
    /// Create a new region of frame.
    /// </summary>
    /// <param name="sequence"><c>null</c> to generate random sequence.</param>
    /// <returns>The <see cref="IFluentOpenBuilder"/> instance.</returns>
    IFluentOpenBuilder Region(int? sequence = default);
}
