namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备激活的参数。
/// </summary>
public interface IHasActive
{
    /// <summary>
    /// 获取或设置一个布尔值，表示是否为激活状态。
    /// </summary>
    bool Active { get; set; }
}
