using System.Linq.Expressions;

namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// 提供构建元素的属性或组件的参数的构造器。
/// </summary>
public interface IFluentAttributeBuilder : IFluentFrameBuilder
{
    /// <summary>
    /// 添加元素属性或组件参数和属性。
    /// </summary>
    /// <param name="name">HTML 属性名称或组件的参数名称。</param>
    /// <param name="value">属性或参数的值。</param>
    /// <returns>包含属性或参数的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is 是 <c>null</c> 或空白字符串。</exception>
    IFluentAttributeBuilder Attribute(string name, object? value);
}

/// <summary>
/// 提供构建组件的参数的构造器。
/// </summary>
/// <typeparam name="TComponent">组件类型。</typeparam>
public interface IFluentAttributeBuilder<TComponent> : IFluentAttributeBuilder, IFluentContentBuilder<TComponent>
    where TComponent : IComponent
{
    /// <summary>
    /// 添加组件的参数和值。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="parameter">参数选择器。</param>
    /// <param name="value">参数的值。</param>
    /// <returns>包含参数的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="parameter"/> 是 <c>null</c>。</exception>
    IFluentAttributeBuilder<TComponent> Attribute<TValue>(Expression<Func<TComponent, TValue>> parameter, TValue? value);
}