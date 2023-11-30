using System.Linq.Expressions;

namespace ComponentBuilder.Definitions;
/// <summary>
/// A binding value that represents the interaction with the user.
/// <para>
/// This interface simplifies the interaction of input components.
/// </para>
/// </summary>
/// <typeparam name="TValue">The type of the bound value.</typeparam>
public interface IHasInputValue<TValue> : IHasValueBound<TValue>
{
    /// <summary>
    /// Gets the cascade parameter <see cref="EditContext"/> from Form.
    /// </summary>
    EditContext? CascadedEditContext { get; }

    /// <summary>
    /// Gets or sets an expression that identifies the bound value.
    /// </summary>
    Expression<Func<TValue?>>? ValueExpression { get; set; }
}
