namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add class attribute.
/// </summary>
internal class CssClassAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnResolvedAttributes(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        var css = component.GetCssClassString();

        if ( !string.IsNullOrEmpty(css) )
        {
            attributes.AddOrUpdate(new("class", css));
        }
    }
}
