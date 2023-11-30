namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add style attribute.
/// </summary>
internal class StyleAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        if(component is not BlazorComponentBase blazorComponent)
        {
            return;
        }
        var style = blazorComponent.GetStyleString();
        if ( style.IsNotNullOrEmpty() )
        {
            attributes.AddOrUpdate(new("style", style!), allowNullValue: false);
        }
    }
}
