namespace ComponentBuilder.Definitions;

/// <summary>
/// 提供具有编辑上下文的组件。
/// </summary>
public interface IHasEditContext
{
    /// <summary>
    /// 获取或设置编辑上下文。
    /// </summary>
    EditContext? EditContext { get; set; }
}
