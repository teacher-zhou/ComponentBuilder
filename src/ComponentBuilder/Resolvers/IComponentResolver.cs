namespace ComponentBuilder.Abstrations;

/// <summary>
/// A component parser that returns results.
/// </summary>
/// <typeparam name="TResult">The resolved result type.</typeparam>
public interface IComponentResolver<out TResult> : IComponentResolver
{
    /// <summary>
    /// Resove component.
    /// </summary>
    /// <param name="component">Component to resolve.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    /// <returns>The resolved result.</returns>
    public TResult Resolve(IBlazorComponent component);
}

/// <summary>
/// The component resolver.
/// </summary>
public interface IComponentResolver { }