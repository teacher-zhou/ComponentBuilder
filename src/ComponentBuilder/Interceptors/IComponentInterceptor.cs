using System.Reflection.Metadata;

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
    void InterceptOnSetParameters(IBlazorComponent component,in ParameterView parameters);
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
    void InterceptOnAfterRender(IBlazorComponent component,in bool firstRender);

    /// <summary>
    /// Intercept when <see cref="BlazorComponentBase.AddContent(RenderTreeBuilder, int)"/> is called.
    /// </summary>
    /// <param name="component">The component to intercept.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    void InterceptOnBuildContent(IBlazorComponent component, RenderTreeBuilder builder, int sequence);

    /// <summary>
    /// Intercept when html attributes are resolved from component.
    /// </summary>
    /// <param name="component">Current instance of component.</param>
    /// <param name="attributes">The html attributes from context.</param>
    void InterceptOnResolvedAttributes(IBlazorComponent component, IDictionary<string, object?> attributes);
    /// <summary>
    /// Intercept when attributes of component or element can be updated.
    /// </summary>
    /// <param name="component">Current instance of component.</param>
    /// <param name="attributes">The html attributes from context.</param>
    void InterceptOnAttributesUpdated(IBlazorComponent component, IDictionary<string, object?> attributes);

    /// <summary>
    /// Intercept when component is disposed.
    /// </summary>
    /// <param name="component">Current instance of component.</param>
    void InterceptOnDispose(IBlazorComponent component);
}
