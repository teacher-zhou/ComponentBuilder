namespace ComponentBuilder;

/// <summary>
/// 提供一个 CSS 类构建器。
/// </summary>
public interface ICssClassBuilder
{
    /// <summary>
    /// 附加指定的 CSS 值。
    /// </summary>
    /// <param name="value">CSS 字符串的值。</param>
    /// <returns>一个包含新值的 <see cref="ICssClassBuilder"/> 实例。</returns>
    ICssClassBuilder Append(string? value);

    /// <summary>
    /// 确定已经包含了已指定 CSS 的值。
    /// </summary>
    /// <param name="value">CSS 字符串的值。</param>
    /// <returns>如果包含该值，则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    bool Contains(string? value);

    /// <summary>
    /// 返回一个 bool 值，表示容器为空。
    /// </summary>
    /// <returns>如果为空，则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
    bool IsEmpty();

    /// <summary>
    /// 将 CSS 值插入到集合的特定索引中。
    /// </summary>
    /// <param name="index">要插入的索引。</param>
    /// <param name="value">要插入的值。</param>
    /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> 超出数组界限。</exception>
    ICssClassBuilder Insert(int index, string? value);
    /// <summary>
    /// 从构建器中移除指定的值。
    /// </summary>
    /// <param name="value">要移除的值。</param>
    ICssClassBuilder Remove(string? value);

    /// <summary>
    /// 清空所有的数据。
    /// </summary>
    void Clear();

    /// <summary>
    /// 将字符串转换为CSS类并连接此构建器中的所有值。
    /// </summary>
    /// <returns>一系列 CSS 字符串，每个条目用空格分隔。</returns>
    string? ToString();
}
