using ComponentBuilder.Abstrations.Internal;

namespace ComponentBuilder;

/// <summary>
/// 一个用于扩展组件 CSS 帮助类的扩展实例。这是一个静态类。
/// </summary>
public static class Css
{
    /// <summary>
    /// 返回一个新实例来构建 CSS 类实用程序。
    /// </summary>
    public static ICssClassProvider Class => new DefaultCssClassProviderBuilder();
}
