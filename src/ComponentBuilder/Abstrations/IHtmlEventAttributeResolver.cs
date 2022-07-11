namespace ComponentBuilder.Abstrations;

/// <summary>
/// 表示对组件参数设置了 <see cref="HtmlEventAttribute"/> 特性的解析器。
/// </summary>
public interface IHtmlEventAttributeResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
