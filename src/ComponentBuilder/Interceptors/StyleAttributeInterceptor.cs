namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add style attribute.
/// </summary>
internal class StyleAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters)
    {
        var style = component.GetStyleString();

        if (!string.IsNullOrEmpty(style))
        {
            component.AdditionalAttributes.AddOrUpdate(new("style", style));
        }
    }
}
