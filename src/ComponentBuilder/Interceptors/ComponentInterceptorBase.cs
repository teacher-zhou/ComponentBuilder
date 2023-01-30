namespace ComponentBuilder.Interceptors;

/// <summary>
/// Provides an empty interceptor base class that implemented from <see cref="IComponentInterceptor"/>.
/// </summary>
public abstract class ComponentInterceptorBase : IComponentInterceptor
{
    /// <summary>
    /// Gets the order of interceptors to invoke from small to larger.
    /// </summary>
    public virtual int Order => 1000;

    /// <inheritdoc/>
    public virtual void InterceptOnResolvedAttributes(IBlazorComponent component, IDictionary<string, object?> attributes)
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
    public virtual void InterceptOnDispose(IBlazorComponent component)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnBuildingContent(IBlazorComponent component, RenderTreeBuilder builder, int sequence)
    {
    }

    /// <inheritdoc/>
    public virtual void InterceptOnAttributesUpdated(IBlazorComponent component, IDictionary<string, object?> attributes)
    {
    }
}
