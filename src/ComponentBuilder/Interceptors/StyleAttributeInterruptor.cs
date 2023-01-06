namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add style attribute.
/// </summary>
internal class StyleAttributeInterruptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnResolvedAttributes(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        var style = component.GetStyleString();

        if ( !string.IsNullOrEmpty(style) )
        {
            attributes.AddOrUpdate(new("style", style));
        }
    }
}
