namespace ComponentBuilder.Parameters;
/// <summary>
/// Provides an additional CSS class of component to append.
/// </summary>
public interface IHasAdditionalCssClass
{
    /// <summary>
    /// The CSS class string to append.
    /// </summary>
    string? AdditionalCssClass { get; set; }
}
