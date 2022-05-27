namespace ComponentBuilder.Abstrations;

/// <summary>
/// Represents a resolver to resolve <see cref="HtmlEventAttribute"/> from component.
/// </summary>
public interface IHtmlEventAttributeResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
