namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides a component has edit context.
/// </summary>
public interface IHasEditContext
{
    /// <summary>
    /// Gets the editing context.
    /// </summary>
    EditContext? EditContext { get; }
}
