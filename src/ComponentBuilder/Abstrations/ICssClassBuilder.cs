namespace ComponentBuilder.Abstrations;

/// <summary>
/// 提供构建 CSS 类的构造器功能。
/// </summary>
public interface ICssClassBuilder : IDisposable
{
    /// <summary>
    /// 追加指定的 CSS 类名称。
    /// </summary>
    /// <param name="value">要追加的 CSS 类名称。</param>
    /// <returns><see cref="ICssClassBuilder"/> 实例。</returns>
    ICssClassBuilder Append(string value);

    /// <summary>
    /// 将指定的值插入集合的指定索引。
    /// </summary>
    /// <param name="index">要插入的索引。</param>
    /// <param name="value">要插入的值。</param>
    /// <returns><see cref="ICssClassBuilder"/> 实例。</returns>
    /// <exception cref="IndexOutOfRangeException">超出索引范围。</exception>
    ICssClassBuilder Insert(int index, string value);

    /// <summary>
    /// 将容器中的 CSS 类转换为字符串。
    /// </summary>
    /// <returns>每个项由空格分隔的字符串。</returns>
    string ToString();
}
