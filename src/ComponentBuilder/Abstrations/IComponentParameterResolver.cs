namespace ComponentBuilder.Abstrations;

/// <summary>
/// Represents a component that can be built.
/// </summary>
/// <typeparam name="TResult">The type of return value.</typeparam>
public interface IComponentParameterResolver<out TResult>
{
    /// <summary>
    /// 解析指定组件。
    /// </summary>
    /// <param name="component">要解析的组件。</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
    public TResult Resolve(ComponentBase component);
}
