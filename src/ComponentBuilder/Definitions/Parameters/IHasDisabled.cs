namespace ComponentBuilder.Definitions;

/// <summary>
/// 提供一个可以被禁用的参数。
/// </summary>
public interface IHasDisabled
{
    /// <summary>
    /// 设置一个布尔值，表示组件的状态为禁用。
    /// </summary>
    [HtmlAttribute] bool Disabled { get; set; }
}
