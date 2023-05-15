using ComponentBuilder.Automation.Builder;

namespace ComponentBuilder.Automation.Definitions;
/// <summary>
/// Provides a parameter to extend CSS class utility.
/// </summary>
public interface IHasCssClassUtility
{
    /// <summary>
    /// The extensions of <see cref="ICssClassUtility"/> instance.
    /// <para>
    /// Normally, you can define the extensions method of <see cref="ICssClassUtility"/> instance for your css class utilties.
    /// </para>
    /// </summary>
    ICssClassUtility? CssClass { get; set; }
}
