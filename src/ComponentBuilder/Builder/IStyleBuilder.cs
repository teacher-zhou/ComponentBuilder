namespace ComponentBuilder;

/// <summary>
/// 提供用于生成样式的容器。
/// </summary>
public interface IStyleBuilder
{
    /// <summary>
    /// 向生成器追加一个新的样式值。
    /// </summary>
    /// <param name="value">样式的值。</param>
    IStyleBuilder Append(string? value);
    /// <summary>
    /// 清除所有值。
    /// </summary>
    void Clear();
    /// <summary>
    /// 将字符串转换为样式并连接此构建器中的所有值。
    /// </summary>
    /// <returns>每个条目由分号(;)分隔的一系列字符串。</returns>
    string ToString();
}
