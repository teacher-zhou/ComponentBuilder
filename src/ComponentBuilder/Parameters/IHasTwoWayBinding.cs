using System.Linq.Expressions;

namespace ComponentBuilder.Parameters;

/// <summary>
/// 提供组件具备双向绑定的功能。
/// </summary>
/// <typeparam name="TValue">要绑定的值类型。</typeparam>
public interface IHasTwoWayBinding<TValue>
{
    /// <summary>
    /// 获取或设置要绑定的值。
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    TValue? Value { get; set; }
    /// <summary>
    /// 获取或设置一个可以识别绑定值的表达式。
    /// </summary>
    Expression<Func<TValue?>> ValueExpression { get; set; }
    /// <summary>
    /// 获取或设置更新绑定值的回调方法。
    /// </summary>
    EventCallback<TValue?> ValueChanged { get; set; }
}
