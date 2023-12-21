namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides a component with an edit context.
/// </summary>
public interface IHasEditContext
{
    /// <summary>
    /// Gets or sets the editing context.
    /// </summary>
    EditContext? EditContext { get; set; }
}
