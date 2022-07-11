using System.Linq.Expressions;

namespace ComponentBuilder.Parameters;

/// <summary>
/// Defines the parameters for component has two-way binding.
/// </summary>
/// <typeparam name="TValue">The value type to bind.</typeparam>
public interface IHasTwoWayBinding<TValue>
{
    /// <summary>
    /// Gets or sets the value to bind.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    TValue? Value { get; set; }
    /// <summary>
    /// Gets or sets an expression that identifies the binding value.
    /// </summary>
    Expression<Func<TValue?>> ValueExpression { get; set; }
    /// <summary>
    /// Gets or sets the callback to update the binding value.
    /// </summary>
    EventCallback<TValue?> ValueChanged { get; set; }
}
