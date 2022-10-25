namespace ComponentBuilder.Abstrations;

/// <summary>
/// A resolver to resolve <see cref="HtmlEventAttribute"/> from component.
/// </summary>
public interface IHtmlEventAttributeResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
