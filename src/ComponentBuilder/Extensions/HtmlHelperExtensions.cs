using ComponentBuilder.Automation.Builder;

namespace ComponentBuilder.Automation;

/// <summary>
/// The helpers of HTML object.
/// </summary>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// Creates a builder for CSS.
    /// </summary>
    public static ICssClassBuilder CreateClass(this HtmlHelper htmlHelper) => new DefaultCssClassBuilder();

    /// <summary>
    /// Creates a builder for style.
    /// </summary>
    public static IStyleBuilder CreateStyle(this HtmlHelper htmlHelper) => new DefaultStyleBuilder();

}
