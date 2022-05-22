namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备激活的参数。
/// </summary>
public interface IHasActive
{
    /// <summary>
    /// Determined a state of component can be actived.
    /// </summary>
    [CssClass("active")] bool Active { get; set; }
}
