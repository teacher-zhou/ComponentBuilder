namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// 提供关闭标记的构造器。
/// </summary>
public interface IFluentCloseBuilder : IDisposable
{
    /// <summary>
    /// 将先前附加的区域、元素或组件标记为关闭。 
    /// 调用该方法时必须与之前的 <c>Element()</c>, <c>Component()</c> 或 <c>Region()</c> 匹配。
    /// </summary>
    IFluentOpenBuilder Close();
}
