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

/// <summary>
/// 提供关闭组件标记的构造器。
/// </summary>
/// <typeparam name="TComponent">组件类型。</typeparam>
public interface IFluentCloseBuilder<TComponent> : IFluentCloseBuilder, IDisposable where TComponent : IComponent
{
    /// <summary>
    /// 将先前附加的区域、元素或组件标记为关闭。 
    /// 调用该方法时必须与之前的 <c>Component()</c> 匹配。
    /// </summary>
    new IFluentOpenComponentBuilder<TComponent> Close();
}