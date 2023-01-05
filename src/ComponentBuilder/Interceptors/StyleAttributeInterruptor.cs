namespace ComponentBuilder.Interceptors;

internal class StyleAttributeInterruptor : ComponentInterceptorBase
{
    public override void InterceptOnResolvedAttributes(IRazorComponent component, IDictionary<string, object> attributes)
    {
        if ( component is not RazorComponentBase razorComponent )
        {
            return;
        }

        var style = razorComponent.GetStyleString();

        if ( !string.IsNullOrEmpty(style) )
        {
            attributes.Update(new("style", style));
        }
    }
}
