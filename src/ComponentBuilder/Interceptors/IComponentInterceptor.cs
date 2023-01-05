namespace ComponentBuilder.Interceptors;

/// <summary>
/// An interceptor with different stage in component lifecyle.
/// </summary>
public interface IComponentInterceptor
{
    /// <summary>
    /// Intercept when <see cref="ComponentBase.SetParametersAsync(ParameterView)"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="parameters"><see cref="ParameterView"/> captured by context.</param>
    void InterceptOnSetParameters(IRazorComponent component,in ParameterView parameters);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnInitialized"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnInitialized(IRazorComponent component);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnParametersSet"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    void InterceptOnParameterSet(IRazorComponent component);
    /// <summary>
    /// Intercept when <see cref="ComponentBase.OnAfterRender(bool)"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="firstRender">A bool value indicates the component rendered in first time.</param>
    void InterceptOnAfterRender(IRazorComponent component,in bool firstRender);

    /// <summary>
    /// Intercept when html attributes are resolved from component.
    /// </summary>
    /// <param name="component">Current instance of component.</param>
    /// <param name="attributes">The html attributes from context.</param>
    void InterceptOnResolvedAttributes(IRazorComponent component, IDictionary<string,object> attributes);

    /// <summary>
    /// Intercept when component is disposed.
    /// </summary>
    /// <param name="component">Current instance of component.</param>
    void InterceptOnDispose(IRazorComponent component);
}
