namespace ComponentBuilder.Interceptors;

/// <summary>
/// 提供一个对 <see cref="IComponentInterceptor"/> 接口空实现的抽象基类。
/// </summary>
public abstract class ComponentInterceptorBase : IComponentInterceptor
{
    /// <summary>
    /// 获取要调用的拦截器，依照从小到大的顺序。
    /// </summary>
    public virtual int Order => 1000;

    /// <inheritdoc/>
    public virtual void InterceptOnAttributesBuilding(IBlazorComponent component, IDictionary<string, object> attributes)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnAfterRender(IBlazorComponent component, in bool firstRender)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnInitialized(IBlazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnParameterSet(IBlazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnSetParameters(IBlazorComponent component, in ParameterView parameters)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnDisposing(IBlazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnContentBuilding(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
    }
}
