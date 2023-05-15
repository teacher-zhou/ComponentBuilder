namespace ComponentBuilder.Automation.Abstrations;

/// <summary>
/// A resolver to resolve <see cref="HtmlAttributeAttribute"/> from compnent.
/// </summary>
public interface IHtmlAttributeResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
{
}
