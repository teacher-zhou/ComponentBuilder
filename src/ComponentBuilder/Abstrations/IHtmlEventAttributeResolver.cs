namespace ComponentBuilder.Abstrations;

/// <summary>
/// Represents a resolver to resolve <see cref="HtmlEventAttribute"/> from parameters in component.
/// </summary>
public interface IHtmlEventAttributeResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
