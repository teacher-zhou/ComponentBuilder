namespace ComponentBuilder.Parameters;
/// <summary>
/// 表示组件之间可以进行切换。
/// </summary>
public interface IHasSwitch
{
    /// <summary>
    /// 获取或设置切换的组件索引。
    /// </summary>
    int? SwitchIndex { get; set; }
}
