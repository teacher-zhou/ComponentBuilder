namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// 表示组件参数的解析器。
    /// </summary>
    /// <typeparam name="TResult">解析后的类型。</typeparam>
    public interface IComponentParameterResolver<TResult>
    {
        /// <summary>
        /// 解析指定组件。
        /// </summary>
        /// <param name="component">要解析的组件。</param>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
        public TResult Resolve(ComponentBase component);
    }
}
