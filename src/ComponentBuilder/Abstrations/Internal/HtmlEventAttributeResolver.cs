//namespace ComponentBuilder.Abstrations.Internal;

///// <summary>
///// Resolve <see cref="HtmlEventAttribute"/> from parameter.
///// </summary>
//internal class HtmlEventAttributeResolver : HtmlAttributeResolverBase
//{
//    /// <inheritdoc/>
//    protected override IEnumerable<KeyValuePair<string, object>> Resolve(ComponentBase component)
//    {
//        var componentType = component.GetType();

//        return componentType.GetInterfaces()
//            .SelectMany(m => m.GetProperties())
//            .GetEventNameValue(component)
//            .Merge(componentType.GetProperties().GetEventNameValue(component));
//        ;
//    }

//}
