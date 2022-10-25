namespace ComponentBuilder.Abstrations;

/// <summary>
/// A resolver to resolve <see cref="HtmlAttributeAttribute"/> from compnent.
/// </summary>
public interface IHtmlAttributesResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
