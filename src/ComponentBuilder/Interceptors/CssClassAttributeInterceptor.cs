namespace ComponentBuilder.Interceptors;

/// <summary>
/// Represents an interceptor to add class attribute.
/// </summary>
internal class CssClassAttributeInterceptor : ComponentInterceptorBase
{
    /// <inheritdoc/>
    public override void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes)
    {
        if (component is not BlazorComponentBase blazorComponent)
        {
            return;
        }

        var value = blazorComponent.GetCssClassString();

        var list = new List<string>();
        if (value.IsNotNullOrEmpty())
        {
            list.Add(value!);
        }

        if (component is IHasAdditionalClass additionalCssClass && !string.IsNullOrWhiteSpace(additionalCssClass.AdditionalClass))
        {
            list.Add(additionalCssClass.AdditionalClass);
        }

        var css = string.Join(" ", list);
        if (css.IsNotNullOrEmpty())
        {
            attributes.AddOrUpdate(new("class", css), allowNullValue: false);
        }
    }

}
