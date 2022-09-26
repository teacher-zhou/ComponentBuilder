namespace ComponentBuilder;

/// <summary>
/// 定义 CSS 类的构造器。
/// </summary>
public interface ICssClassBuilder : IDisposable
{
    /// <summary>
    /// 向构建器添加特定的CSS值。
    /// </summary>
    /// <param name="value">CSS 类的值。</param>
    /// <returns>一个包含值的 <see cref="ICssClassBuilder"/> 实例。</returns>
    ICssClassBuilder Append(string? value);

    /// <summary>
    /// 判断是否包含指定的值。
    /// </summary>
    /// <param name="value">要判断的 CSS 类的值。</param>
    /// <returns>若包含该值，则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
    bool Contains(string? value);

    /// <summary>
    /// 获取一个布尔值，表示 CSS 构造器是否为空。
    /// </summary>
    /// <returns>如果是空，则返回 <c>true</c>；否则返回 <c>false</c>。</returns>
    bool IsEmpty();

    /// <summary>
    /// 在这个构建器中，将CSS值插入集合的特定索引中。
    /// </summary>
    /// <param name="index">要插入的索引。</param>
    /// <param name="value">CSS 类的值。</param>
    /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> 超出数组界限。</exception>
    /// <returns>一个包含值的 <see cref="ICssClassBuilder"/> 实例。</returns>
    ICssClassBuilder Insert(int index, string? value);
    /// <summary>
    /// 从构造器中移除指定的值。
    /// </summary>
    /// <param name="value">要移除的 CSS 的值。</param>
    /// <returns>一个移除值的 <see cref="ICssClassBuilder"/> 实例。</returns>
    ICssClassBuilder Remove(string? value);

    /// <summary>
    /// 将字符串转换为CSS类，并在此构建器中连接所有值。
    /// </summary>
    /// <returns>表示CSS类的字符串，每一项用空格隔开。</returns>
    string? ToString();
}
