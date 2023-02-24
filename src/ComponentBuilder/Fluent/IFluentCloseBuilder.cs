namespace ComponentBuilder.Fluent;
/// <summary>
/// Provides an close frame of render tree.
/// </summary>
public interface IFluentCloseBuilder : IDisposable
{
    /// <summary>
    /// Marks a previously appended region, element or component as closed. Calls to this method
    /// must be balanced with calls to <c>Element()</c>, <c>Component()</c> or <c>Region()</c>.
    /// </summary>
    IFluentOpenBuilder Close();
}
