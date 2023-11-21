namespace ComponentBuilder.Interceptors;

/// <summary>
/// Provides an abstract base class for an empty implementation of the <see cref="IComponentInterceptor"/> interface.
/// </summary>
public abstract class ComponentInterceptorBase : IComponentInterceptor
{
    /// <inheritdoc/>
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
