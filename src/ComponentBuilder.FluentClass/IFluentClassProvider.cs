namespace ComponentBuilder.FluentClass;
/// <summary>
/// Provides a provider for fluent css class from parameter.
/// </summary>
public interface IFluentClassProvider
{
    /// <summary>
    /// Create a series string for CSS class.
    /// </summary>
    /// <returns>A collection of string representing CSS class.</returns>
    IEnumerable<string> Create();
}
