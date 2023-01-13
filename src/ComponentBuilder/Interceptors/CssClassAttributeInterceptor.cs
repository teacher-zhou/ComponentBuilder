namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add class attribute.
/// </summary>
internal class CssClassAttributeInterceptor : ComponentInterceptorBase
{
    public override void InterceptOnInitialized(IBlazorComponent component)
    {
        component.CssClassBuilder.Clear();
    }

    /// <inheritdoc/>
    public override void InterceptOnUpdatingAttributes(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        var css = component.GetCssClassString();

        if ( !string.IsNullOrEmpty(css) )
        {
            attributes.AddOrUpdate(new("class", css));
        }
    }
}
