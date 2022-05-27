namespace ComponentBuilder.Abstrations;

/// <summary>
/// Represents a resolver to resolve <see cref="HtmlAttributeAttribute"/> for component.
/// </summary>
public interface IHtmlAttributesResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
