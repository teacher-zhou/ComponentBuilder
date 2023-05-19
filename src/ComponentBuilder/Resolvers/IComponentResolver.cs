namespace ComponentBuilder.Abstrations;

/// <summary>
/// 具备返回结果的组件解析器。
/// </summary>
/// <typeparam name="TResult">解析结果。</typeparam>
public interface IComponentResolver<out TResult> : IComponentResolver
{
    /// <summary>
    /// 解析指定组件。
    /// </summary>
    /// <param name="component">要解析的组件。</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
    /// <returns>解析后的结果。</returns>
    public TResult Resolve(IBlazorComponent component);
}

/// <summary>
/// 组件解析器。
/// </summary>
public interface IComponentResolver { }