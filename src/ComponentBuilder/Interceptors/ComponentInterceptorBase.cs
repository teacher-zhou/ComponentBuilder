namespace ComponentBuilder.Interceptors;

/// <summary>
/// Provides an empty interceptor base class that implemented from <see cref="IComponentInterceptor"/>.
/// </summary>
public abstract class ComponentInterceptorBase : IComponentInterceptor
{
    /// <inheritdoc/>
    public virtual void InterceptOnResolvedAttributes(IBlazorComponent component, IDictionary<string, object> attributes)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnAfterRender(IBlazorComponent component,in bool firstRender)
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
    public virtual void InterceptOnSetParameters(IBlazorComponent component,in ParameterView parameters)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnDispose(IBlazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnBuildContent(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
    }
}
