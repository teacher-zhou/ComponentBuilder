namespace ComponentBuilder.Definitions;
/// <summary>
/// 为组件定义一个附加属性，以捕获不匹配的 html 属性值。
/// </summary>
public interface IHasAdditionalAttributes
{
    /// <summary>
    /// 获取或设置元素中的附加属性，该属性可自动捕获不匹配的 html 属性值。
    /// </summary>
    IDictionary<string, object> AdditionalAttributes { get; set; }
}
