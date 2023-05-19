using System.Linq.Expressions;

namespace ComponentBuilder.Definitions;

/// <summary>
/// 为组件提供双向绑定功能。
/// </summary>
/// <typeparam name="TValue">值的类型。</typeparam>
public interface IHasValueBound<TValue>:IBlazorComponent
{
    /// <summary>
    /// 获取或设置要绑定的值。
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    TValue? Value { get; set; }
    /// <summary>
    /// 获取或设置更新绑定值的回调方法。
    /// </summary>
    EventCallback<TValue?> ValueChanged { get; set; }
}
