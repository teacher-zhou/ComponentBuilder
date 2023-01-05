namespace ComponentBuilder.Interceptors;

/// <summary>
/// Provides an empty interceptor base class that implemented from <see cref="IComponentInterceptor"/>.
/// </summary>
public abstract class ComponentInterceptorBase : IComponentInterceptor
{
    /// <inheritdoc/>
    public virtual void InterceptOnResolvedAttributes(IRazorComponent component, IDictionary<string, object> attributes)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnAfterRender(IRazorComponent component,in bool firstRender)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnInitialized(IRazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnParameterSet(IRazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnSetParameters(IRazorComponent component,in ParameterView parameters)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnDispose(IRazorComponent component)
    {
    }
}
