namespace ComponentBuilder.Abstrations;

/// <summary>
/// 该接口用于组件框架对常用的 CSS 类名称进行扩展使用。
/// </summary>
public interface ICssClassUtility
{
    /// <summary>
    /// 追加指定 CSS 字符串。
    /// </summary>
    /// <param name="value">要追加的 CSS 名称。</param>
    /// <returns>实现了 <see cref="ICssClassUtility"/> 的实例、</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> 是 null.</exception>
    ICssClassUtility Append(string value);
    /// <summary>
    /// 返回 CSS 名称的集合。
    /// </summary>
    IEnumerable<string> CssClasses { get; }
}
