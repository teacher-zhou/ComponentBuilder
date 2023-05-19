namespace ComponentBuilder.Definitions;
/// <summary>
/// 表示可以在组件之间切换。
/// </summary>
public interface IHasSwitch
{
    /// <summary>
    /// 获取或设置切换组件索引。
    /// </summary>
    int? SwitchIndex { get; set; }
}
