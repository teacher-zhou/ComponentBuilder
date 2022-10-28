namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// A base class for component to resolve parameters.
/// </summary>
/// <typeparam name="TResult">The type of result.</typeparam>
internal abstract class ComponentParameterResolverBase<TResult> : IComponentParameterResolver<TResult>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
    /// <exception cref="InvalidCastException"><paramref name="component"/> is not <see cref="ComponentBase"/>.</exception>
    public TResult Resolve(object component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        if (component is not ComponentBase componentBase)
        {
            throw new InvalidCastException($"要解析的组件必须是 {nameof(ComponentBase)} 类型");
        }
        return Resolve(componentBase);
    }
    
    /// <summary>
    /// Resolve specifed component.
    /// </summary>
    /// <param name="component">The component to resolve.</param>
    protected abstract TResult Resolve(ComponentBase component);
}
