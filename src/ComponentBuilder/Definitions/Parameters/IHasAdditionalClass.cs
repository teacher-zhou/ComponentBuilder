namespace ComponentBuilder.Definitions;
/// <summary>
/// Provides additional CSS classes for the components to append.
/// </summary>
public interface IHasAdditionalClass
{
    /// <summary>
    /// Gets or sets the string of additional CSS classes to append.
    /// <para>
    /// Typically, this value is appended to the end of all CSS parameters after they have been built.
    /// </para>
    /// </summary>
    string? AdditionalClass { get; set; }
}
