namespace ComponentBuilder.Definitions;
/// <summary>
/// 提供要追加的组件的附加 CSS 类。
/// </summary>
public interface IHasAdditionalClass
{
    /// <summary>
    /// 获取或设置要追加的其他 CSS class 的字符串。
    /// <para>
    /// 通常，这个值会追加到所有 CSS 参数完成构建后的后面。
    /// </para>
    /// </summary>
    string? AdditionalClass { get; set; }
}
