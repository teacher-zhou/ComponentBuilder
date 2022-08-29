namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备可禁用的参数。
/// </summary>
public interface IHasDisabled
{
    /// <summary>
    /// 获取或设置一个布尔值，表示是否为禁用状态。
    /// </summary>
    bool Disabled { get; set; }
}
