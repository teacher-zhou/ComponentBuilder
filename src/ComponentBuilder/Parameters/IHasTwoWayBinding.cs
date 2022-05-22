using System.Linq.Expressions;

namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件来支持用户交互的双向绑定功能。
/// </summary>
/// <typeparam name="TValue">绑定的值类型。</typeparam>
public interface IHasTwoWayBinding<TValue>
{
    /// <summary>
    /// 获取或设置输入的值。这应该与双向绑定一起使用。
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    TValue? Value { get; set; }
    /// <summary>
    /// 获取或设置标识绑定值的表达式。
    /// </summary>
    Expression<Func<TValue?>> ValueExpression { get; set; }
    /// <summary>
    /// 获取或设置更新绑定值的回调。
    /// </summary>
    EventCallback<TValue?> ValueChanged { get; set; }
}
