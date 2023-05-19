namespace ComponentBuilder.Interceptors;

/// <summary>
/// 提供在组件生命周期不同阶段的拦截功能。
/// </summary>
public interface IComponentInterceptor
{
    /// <summary>
    /// 获取拦截器的顺序。
    /// </summary>
    int Order { get; }
    /// <summary>
    /// 在 <see cref="ComponentBase.SetParametersAsync(ParameterView)"/> 阶段执行的拦截操作。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    /// <param name="parameters">被上下文捕获的 <see cref="ParameterView"/> 对象。</param>
    void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters);
    /// <summary>
    /// 在 <see cref="ComponentBase.OnInitialized"/> 阶段执行的拦截操作。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    void InterceptOnInitialized(IBlazorComponent component);
    /// <summary>
    /// 在 <see cref="ComponentBase.OnParametersSet"/> 阶段执行的拦截操作。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    void InterceptOnParameterSet(IBlazorComponent component);
    /// <summary>
    /// 在 <see cref="ComponentBase.OnAfterRender(bool)"/> 阶段执行的拦截操作。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    /// <param name="firstRender">一个布尔值值，表示组件是第一次呈现。</param>
    void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender);

    /// <summary>
    /// 在 <see cref="BlazorComponentBase.AddContent(RenderTreeBuilder, int)"/> 阶段执行的拦截操作。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 的实例。</param>
    /// <param name="sequence">表示源代码序列的整数。</param>
    void InterceptOnContentBuilding(IBlazorComponent component, RenderTreeBuilder builder, int sequence);

    /// <summary>
    /// 当组件正在构建 html 属性时进行拦截。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    /// <param name="attributes">构建前的 HTML 属性。</param>
    void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes);
    /// <summary>
    /// 当组件被释放时进行拦截。
    /// </summary>
    /// <param name="component">要拦截的组件。</param>
    void InterceptOnDisposing(IBlazorComponent component);
}
