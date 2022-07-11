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
    ICssClassBuilder Append(string value);

    /// <summary>
    /// 在这个构建器中，将CSS值插入集合的特定索引中。
    /// </summary>
    /// <param name="index">要插入的索引。</param>
    /// <param name="value">CSS 类的值。</param>
    /// <exception cref="IndexOutOfRangeException"> <paramref name="index"/> 超出数组界限。</exception>
    /// <returns>一个包含值的 <see cref="ICssClassBuilder"/> 实例。</returns>
    ICssClassBuilder Insert(int index, string value);

    /// <summary>
    /// 将字符串转换为CSS类，并在此构建器中连接所有值。
    /// </summary>
    /// <returns>表示CSS类的字符串，每一项用空格隔开。</returns>
    string ToString();
}
