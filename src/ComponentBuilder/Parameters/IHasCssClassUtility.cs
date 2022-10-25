namespace ComponentBuilder.Parameters;
/// <summary>
/// Provides a parameter to extend CSS class utility.
/// </summary>
public interface IHasCssClassUtility
{
    /// <summary>
    /// The extensions of <see cref="ICssClassUtility"/> instance.
    /// </summary>
    ICssClassUtility? CssClass { get; set; }
}
