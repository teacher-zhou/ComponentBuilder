namespace ComponentBuilder.Abstrations;

/// <summary>
/// 提供构造组件样式的功能。
/// </summary>
public interface IStyleBuilder : IDisposable
{
    /// <summary>
    /// 追加指定的样式。
    /// </summary>
    /// <param name="value">要追加样式的值。</param>
    /// <returns><see cref="IStyleBuilder"/> 的实例。</returns>
    IStyleBuilder Append(string value);
    /// <summary>
    /// 转换为由';' char分隔的样式字符串。
    /// </summary>
    /// <returns>表示 HTML 样式的字符串。</returns>
    string ToString();
}
