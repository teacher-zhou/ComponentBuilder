namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// 提供用于创建新范围的构造器。
/// </summary>
public interface IFluentRegionBuilder:IDisposable
{
    /// <summary>
    /// 创建一个新的范围。
    /// </summary>
    /// <param name="sequence">一个新的源代码位置的起始范围。</param>
    /// <returns><see cref="IFluentOpenBuilder"/> 实例。</returns>
    IFluentOpenBuilder Region(int sequence);
}