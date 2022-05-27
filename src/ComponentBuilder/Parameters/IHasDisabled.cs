namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备可禁用的参数。
/// </summary>
public interface IHasDisabled
{
    /// <summary>
    /// Determined a state of component can be disabled.
    /// </summary>
    [CssClass("disabled")] bool Disabled { get; set; }
}
