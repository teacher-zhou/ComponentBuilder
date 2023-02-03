namespace ComponentBuilder.Interceptors;

/// <summary>
/// An interceptor with different stage in component lifecyle.
/// </summary>
public interface IComponentInterceptor
{
    /// <summary>
    /// Gets the order of interceptor.
    /// </summary>
    int Order { get; }
    /// <summary>
    /// Intercept when <see cref="ComponentBase.SetParametersAsync(ParameterView)"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="parameters"><see cref="ParameterView"/> captured by context.</param>
    void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnInitialized"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnInitialized(IBlazorComponent component);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnParametersSet"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnParameterSet(IBlazorComponent component);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnAfterRender(bool)"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="firstRender">A bool value indicates the component rendered in first time.</param>
    void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender);

    /// <summary>
    /// Intercept when <see cref="BlazorComponentBase.AddContent(RenderTreeBuilder, int)"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    void InterceptOnBuildingContent(IBlazorComponent component, RenderTreeBuilder builder, int sequence);

    /// <summary>
    /// Intercept when component is building html attributes.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="attributes">The collection of attributes.</param>
    void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes);
    /// <summary>
    /// Intercept when component is disposed.
    /// </summary>
    /// <param name="component">Current instance of component.</param>
    void InterceptOnDispose(IBlazorComponent component);
}
