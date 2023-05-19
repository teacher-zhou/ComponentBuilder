using System.Linq.Expressions;

namespace ComponentBuilder.Definitions;
/// <summary>
/// 表示与用户交互的绑定值。
/// <para>
/// 该接口可以简化输入组件的交互。
/// </para>
/// </summary>
/// <typeparam name="TValue">绑定的值的类型。</typeparam>
public interface IHasInputValue<TValue> : IHasValueBound<TValue>
{
    /// <summary>
    /// 从表单组件获取级联 <see cref="EditContext"/>。
    /// </summary>
    EditContext? CascadedEditContext { get; }

    /// <summary>
    /// 获取或设置识别绑定值的表达式。
    /// </summary>
    Expression<Func<TValue?>>? ValueExpression { get; set; }
}
