namespace ComponentBuilder.FluentClass;
/// <summary>
/// 从参数提供流畅 CSS 类的提供程序。
/// </summary>
public interface IFluentClassProvider
{
    /// <summary>
    /// 为CSS类创建一个系列字符串。
    /// </summary>
    /// <returns>表示CSS类的字符串集合。</returns>
    IEnumerable<string> Create();
}
