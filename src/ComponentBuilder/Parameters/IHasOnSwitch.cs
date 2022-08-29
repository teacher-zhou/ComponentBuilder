namespace ComponentBuilder.Parameters;
/// <summary>
/// 提供组件可以被进行切换的函数。
/// </summary>
public interface IHasOnSwitch : IHasSwitch, IRefreshableComponent
{
    /// <summary>
    /// 设置指定组件索引的可切换的回调方法。
    /// </summary>
    EventCallback<int?> OnSwitch { get; set; }
}
