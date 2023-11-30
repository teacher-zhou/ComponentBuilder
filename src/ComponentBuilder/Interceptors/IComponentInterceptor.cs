namespace ComponentBuilder.Interceptors;

/// <summary>
/// Provides interception at different stages of the component lifecycle.
/// </summary>
public interface IComponentInterceptor
{
    /// <summary>
    /// The order in which interceptors are obtained.
    /// </summary>
    int Order { get; }
    /// <summary>
    /// Intercept when <see cref="ComponentBase.SetParametersAsync(ParameterView)"/> calling.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="parameters">The <see cref="ParameterView"/> object captured by context.</param>
    void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnInitialized"/> calling.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnInitialized(IBlazorComponent component);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnParametersSet"/> calling.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnParameterSet(IBlazorComponent component);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnAfterRender(bool)"/> calling.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="firstRender">A Boolean value indicating that the component was first rendered.</param>
    void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender);

    /// <summary>
    /// Intercept when <see cref="BlazorComponentBase.AddContent(RenderTreeBuilder, int)"/> calling.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void InterceptOnContentBuilding(IBlazorComponent component, RenderTreeBuilder builder, int sequence);

    /// <summary>
    /// Intercept when the component is building an html property.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="attributes">HTML property before build.</param>
    void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object?> attributes);
    /// <summary>
    /// Intercept when disposing component.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnDisposing(IBlazorComponent component);
}
