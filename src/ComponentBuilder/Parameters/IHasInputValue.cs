using System.Linq.Expressions;

namespace ComponentBuilder.Parameters;
/// <summary>
/// Represents a bound value interacting with user.
/// <para>
/// The interface could simplified input component for interaction.
/// </para>
/// </summary>
/// <typeparam name="TValue">The type of value.</typeparam>
public interface IHasInputValue<TValue>:IHasValueBound<TValue>,IHasEditContext
{
    /// <summary>
    /// Gets or sets an expression that recognizes the bound value.
    /// </summary>
    Expression<Func<TValue?>>? ValueExpression { get; set; }

    /// <summary>
    /// Gets the cascading <see cref="EditContext"/> from form component.
    /// </summary>
    EditContext? CascadedEditContext { get; }
}
