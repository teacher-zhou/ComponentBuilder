namespace ComponentBuilder.Abstrations;

/// <summary>
/// 该接口可以用于扩展 CSS 工具类的方法使用。
/// </summary>
public interface ICssClassUtility
{
    /// <summary>
    /// 追加 CSS 字符串。
    /// </summary>
    /// <param name="value">追加的 CSS 字符串。</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> 是 <c>null</c>。</exception>
    ICssClassUtility Append(string value);
    /// <summary>
    /// 返回一系列的 CSS 了名称集合。
    /// </summary>
    IEnumerable<string> CssClasses { get; }
}
