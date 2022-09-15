namespace ComponentBuilder.Abstrations.Internal;

/// <summary>
/// 组件参数解析器。
/// </summary>
/// <typeparam name="TResult"></typeparam>
public abstract class ComponentParameterResolver<TResult> : IComponentParameterResolver<TResult>
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="component"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidCastException"></exception>
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
    /// 解析组件。
    /// </summary>
    /// <param name="component">要解析的组件。</param>
    /// <returns></returns>
    protected abstract TResult Resolve(ComponentBase component);
}
