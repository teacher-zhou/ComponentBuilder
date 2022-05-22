namespace ComponentBuilder.Abstrations;

/// <summary>
/// 提供对 <see cref="HtmlAttributeAttribute"/> 特性的解析器。
/// </summary>
public interface IHtmlAttributesResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
