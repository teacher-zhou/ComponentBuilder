namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add style attribute.
/// </summary>
internal class StyleAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        var style = component.GetStyleString();
        if ( style.IsNotNullOrEmpty() )
        {
            attributes.AddOrUpdate(new("style", style!), allowNullValue: false);
        }
    }
}
