namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Resolves attributes of property value for element from parameters in component.
    /// </summary>
    public interface IElementAttributesResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
    {
    }
}
