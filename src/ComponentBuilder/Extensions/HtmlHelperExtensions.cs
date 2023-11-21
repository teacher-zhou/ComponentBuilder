using ComponentBuilder.Builder;

namespace ComponentBuilder;

/// <summary>
/// <see cref="HtmlHelper"/> extensions.
/// </summary>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// Creates <see cref="ICssClassBuilder"/> instance.
    /// </summary>
    public static ICssClassBuilder Class(this HtmlHelper htmlHelper) => new DefaultCssClassBuilder();

    /// <summary>
    /// Creates <see cref="IStyleBuilder"/> instance.
    /// </summary>
    public static IStyleBuilder Style(this HtmlHelper htmlHelper) => new DefaultStyleBuilder();

}
