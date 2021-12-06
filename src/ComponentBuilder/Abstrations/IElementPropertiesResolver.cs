namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Provides to resolve attributes of element from parameters in component.
    /// </summary>
    public interface IElementPropertiesResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
    {
    }
}
