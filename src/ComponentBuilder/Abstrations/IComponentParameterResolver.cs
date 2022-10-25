namespace ComponentBuilder.Abstrations;

/// <summary>
/// A resolver for component parameters.
/// </summary>
/// <typeparam name="TResult">The type of result.</typeparam>
public interface IComponentParameterResolver<out TResult>
{
    /// <summary>
    /// Resolve specified component.
    /// </summary>
    /// <param name="component">The component to resolve.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    /// <returns>The result after resolve.</returns>
    public TResult Resolve(object component);
}
