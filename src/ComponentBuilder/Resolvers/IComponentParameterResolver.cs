namespace ComponentBuilder.Automation.Abstrations;

/// <summary>
/// A resolver for component parameters.
/// </summary>
/// <typeparam name="TResult">The type of result.</typeparam>
public interface IComponentParameterResolver<out TResult>:IComponentResolver
{
    /// <summary>
    /// Resolve specified component.
    /// </summary>
    /// <param name="component">The component to resolve.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    /// <returns>The result after resolve.</returns>
    public TResult Resolve(IBlazorComponent component);
}

/// <summary>
/// A resolver for component.
/// </summary>
public interface IComponentResolver { }