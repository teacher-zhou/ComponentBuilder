using ComponentBuilder.Builder;

namespace ComponentBuilder;

/// <summary>
/// The helpers of HTML object.
/// </summary>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// Creates a builder for CSS.
    /// </summary>
    public static ICssClassBuilder Class(this HtmlHelper htmlHelper) => new DefaultCssClassBuilder();

    /// <summary>
    /// Creates a builder for style.
    /// </summary>
    public static IStyleBuilder Style(this HtmlHelper htmlHelper) => new DefaultStyleBuilder();

}
