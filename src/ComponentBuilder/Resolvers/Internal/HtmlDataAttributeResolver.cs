//using System.Reflection;

//namespace ComponentBuilder.Abstrations.Internal;

///// <summary>
///// Resolve <see cref="HtmlDataAttribute"/> from parameter.
///// </summary>
//internal class HtmlDataAttributeResolver : HtmlAttributeAttributeResolver
//{
//    /// <inheritdoc/>
//    protected override IEnumerable<KeyValuePair<string, object>> Resolve(ComponentBase component)
//    {
//        if (component is null)
//        {
//            throw new ArgumentNullException(nameof(component));
//        }

//        var attributes = GetHtmlAttributes<HtmlDataAttribute>(component);


//        return attributes;
//    }
//}
