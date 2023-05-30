using ComponentBuilder.Builder;

namespace ComponentBuilder;

/// <summary>
/// <see cref="HtmlHelper"/> 的扩展。
/// </summary>
public static class HtmlHelperExtensions
{
    /// <summary>
    /// 获取 <see cref="ICssClassBuilder"/> 实例。
    /// </summary>
    public static ICssClassBuilder Class(this HtmlHelper htmlHelper) => new DefaultCssClassBuilder();

    /// <summary>
    /// 获取 <see cref="IStyleBuilder"/> 实例。
    /// </summary>
    public static IStyleBuilder Style(this HtmlHelper htmlHelper) => new DefaultStyleBuilder();

}
