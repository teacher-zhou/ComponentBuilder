namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// A resolver to resolve parameters from component defined <see cref="HtmlEventAttribute"/> attribute.
/// </summary>
internal class HtmlEventAttributeResolver : ComponentParameterResolverBase<IEnumerable<KeyValuePair<string, object>>>, IHtmlEventAttributeResolver
{
    /// <inheritdoc/>
    protected override IEnumerable<KeyValuePair<string, object>> Resolve(ComponentBase component)
    {
        var componentType = component.GetType();

        return componentType.GetInterfaces()
            .SelectMany(m => m.GetProperties())
            .GetEventNameValue(component)
            .Merge(componentType.GetProperties().GetEventNameValue(component));
        ;
    }

}
