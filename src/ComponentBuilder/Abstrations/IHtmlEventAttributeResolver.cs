namespace ComponentBuilder.Abstrations;

/// <summary>
/// 表示可以对 <see cref="HtmlEventAttribute"/> 进行解析的解析器。
/// </summary>
public interface IHtmlEventAttributeResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
