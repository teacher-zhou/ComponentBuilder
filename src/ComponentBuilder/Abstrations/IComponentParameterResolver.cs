namespace ComponentBuilder.Abstrations;

/// <summary>
/// 定义对组件参数的解析器。
/// </summary>
/// <typeparam name="TResult">解析的结果类型。</typeparam>
public interface IComponentParameterResolver<out TResult>
{
    /// <summary>
    /// 解析指定组件。
    /// </summary>
    /// <param name="component">要解析的组件。</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
    public TResult Resolve(object component);
}
