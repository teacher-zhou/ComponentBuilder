namespace ComponentBuilder.Abstrations;

/// <summary>
/// Provides to resolve <see cref="CssClassAttribute"/> from component to create CSS class string.
/// </summary>
public interface ICssClassAttributeResolver : IComponentParameterResolver<string>
{
}
