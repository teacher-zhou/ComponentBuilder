using ComponentBuilder.Automation.Builder;

namespace ComponentBuilder.Automation;

/// <summary>
/// A <see langword="static"/> class for <see cref="ICssClassUtility"/> extensions.
/// </summary>
public static class Css
{
    /// <summary>
    /// Return a new instance to build the CSS class utility.
    /// <para>
    /// Use extension methods for <see cref="ICssClassUtility"/> to extend CSS class utilities.
    /// </para>
    /// </summary>
    public static ICssClassUtility Class => new DefaultCssClassUtilityBuilder();
}
