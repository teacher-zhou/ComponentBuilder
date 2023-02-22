namespace ComponentBuilder.Fluent;
/// <summary>
/// Provides an close frame of element or component.
/// </summary>
public interface IFluentCloseBuilder : IDisposable
{
    /// <summary>
    /// Marks a previously appended element or component as closed. Calls to this method
    /// must be balanced with calls to <c>Element()</c> or <c>Component</c>.
    /// </summary>
    RenderTreeBuilder Close();
}
