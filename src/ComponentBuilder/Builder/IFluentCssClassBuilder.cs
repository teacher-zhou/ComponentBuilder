namespace ComponentBuilder;
/// <summary>
/// Provides a builder for fluent css class from parameter.
/// </summary>
public interface IFluentCssClassBuilder
{
    /// <summary>
    /// Build a string representing a series of CSS class.
    /// </summary>
    /// <returns>A CSS class string seperated by space for each item.</returns>
    string? Build();
}
