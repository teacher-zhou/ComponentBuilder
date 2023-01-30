namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add class attribute.
/// </summary>
internal class CssClassAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnResolvingAttributes(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
        var css = component.GetCssClassString();
        if ( css.IsNotNullOrEmpty() )
        {
            attributes.AddOrUpdate(new("class", css), allowNullValue: false);
        }
    }
}
