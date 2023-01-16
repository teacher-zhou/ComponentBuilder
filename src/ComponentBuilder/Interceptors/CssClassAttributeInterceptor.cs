namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add class attribute.
/// </summary>
internal class CssClassAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters)
    {
        var css = component.GetCssClassString();

        if (!string.IsNullOrEmpty(css))
        {
            component.AdditionalAttributes.AddOrUpdate(new("class", css));
        }
    }
}
