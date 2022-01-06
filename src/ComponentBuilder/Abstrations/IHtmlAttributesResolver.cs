namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Resolves html element attributes from parameters in component.
    /// </summary>
    public interface IHtmlAttributesResolver : IComponentParameterResolver<IEnumerable<KeyValuePair<string, object>>>
    {
    }
}
