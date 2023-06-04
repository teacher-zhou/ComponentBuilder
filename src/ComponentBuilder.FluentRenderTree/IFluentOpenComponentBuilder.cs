namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// 提供创建组件的构造器功能。
/// </summary>
public interface IFluentOpenComponentBuilder : IDisposable
{
    /// <summary>
    /// 创建指定的实现 <see cref="IComponent"/> 接口组件类型的组件开始标记。
    /// </summary>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个表示源代码的序列。<c>null</c> 则系统会自动创建。</param>
    /// <returns><see cref="IFluentAttributeBuilder"/> 的实例。</returns>
    IFluentAttributeBuilder Component(Type componentType, int? sequence = default);
}

/// <summary>
/// 提供创建指定组件类型的构造器功能。
/// </summary>
/// <typeparam name="TComponent">组件的类型。</typeparam>
public interface IFluentOpenComponentBuilder<TComponent> : IFluentOpenComponentBuilder
    where TComponent : IComponent
{
    /// <summary>
    /// 创建组件的开始标记。
    /// </summary>
    /// <param name="sequence">一个表示源代码的序列。<c>null</c> 则系统会自动创建。</param>
    /// <returns><see cref="IFluentAttributeBuilder"/> 的实例。</returns>
    IFluentAttributeBuilder<TComponent> Component(int? sequence = default);
}