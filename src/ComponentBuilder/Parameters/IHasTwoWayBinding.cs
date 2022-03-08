using System.Linq.Expressions;

namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides component to support two-way bindings for interaction by user.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
public interface IHasTwoWayBinding<TValue>
{
    /// <summary>
    /// Gets or sets the value of the input. This should be used with two-way binding.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    TValue Value { get; set; }
    /// <summary>
    /// Gets or sets an expression that identifies the bound value.
    /// </summary>
    Expression<Func<TValue>> ValueExpression { get; set; }
    /// <summary>
    /// Gets or sets a callback that updates the bound value.
    /// </summary>
    EventCallback<TValue> ValueChanged { get; set; }
}
