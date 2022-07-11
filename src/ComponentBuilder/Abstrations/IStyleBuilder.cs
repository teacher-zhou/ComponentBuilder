namespace ComponentBuilder;

/// <summary>
/// 定义 style 样式的构造器。
/// </summary>
public interface IStyleBuilder : IDisposable
{
    /// <summary>
    /// 向构建器添加特定的 style 值。
    /// </summary>
    /// <param name="value">style 的值。</param>
    /// <returns>一个包含值的 <see cref="IStyleBuilder"/> 实例。</returns>
    IStyleBuilder Append(string value);
    /// <summary>
    /// 将字符串转换为 style，并在此构建器中连接所有值。
    /// </summary>
    /// <returns>表示样式的字符串，每一项以分号(;)分隔。</returns>
    string ToString();
}
