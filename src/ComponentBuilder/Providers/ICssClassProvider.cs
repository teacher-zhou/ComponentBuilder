namespace ComponentBuilder;

/// <summary>
/// 该接口可以用于扩展特殊的 CSS 类的方法使用。
/// </summary>
public interface ICssClassProvider
{
    /// <summary>
    /// 追加 CSS 字符串。
    /// </summary>
    /// <param name="value">追加的 CSS 字符串。</param>
    ICssClassProvider Append(string value);
    /// <summary>
    /// 返回一系列的 CSS 了名称集合。
    /// </summary>
    IEnumerable<string> CssClasses { get; }
}
