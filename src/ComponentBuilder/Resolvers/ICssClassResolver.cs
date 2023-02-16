namespace ComponentBuilder.Abstrations;

/// <summary>
/// A resolve to resolve <see cref="CssClassAttribute"/> from component.
/// </summary>
public interface ICssClassResolver : IComponentParameterResolver<IEnumerable<string>>
{
}
