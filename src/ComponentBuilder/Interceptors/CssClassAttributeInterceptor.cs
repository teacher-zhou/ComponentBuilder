namespace ComponentBuilder.Interceptors;

internal class CssClassAttributeInterceptor : ComponentInterceptorBase
{
    public override void InterceptOnResolvedAttributes(IRazorComponent component, IDictionary<string, object> attributes)
    {
        if ( component is not RazorComponentBase razorComponent )
        {
            return;
        }

        var css = razorComponent.GetCssClassString();

        if ( !string.IsNullOrEmpty(css) )
        {
            attributes.Update(new("class", css));
        }
    }
}
