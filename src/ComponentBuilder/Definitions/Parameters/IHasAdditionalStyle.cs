namespace ComponentBuilder.Definitions;
/// <summary>
/// 提供要追加的组件的附加样式。
/// </summary>
public interface IHasAdditionalStyle
{
    /// <summary>
    /// 获取或设置要追加的其他样式字符串。
    /// <para>
    /// 通常，这个值会追加到所有 style 参数完成构建后的后面。
    /// </para>    
    /// </summary>
    string? AdditionalStyle { get; set; }
}
