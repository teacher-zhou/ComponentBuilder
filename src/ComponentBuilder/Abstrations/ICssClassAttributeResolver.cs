namespace ComponentBuilder.Abstrations;

/// <summary>
/// The resolver witch can resolve <see cref="CssClassAttribute"/> for component.
/// </summary>
public interface ICssClassAttributeResolver : IComponentParameterResolver<string>
{
}
