using System.Linq.Expressions;

namespace ComponentBuilder.Parameters;

/// <summary>
/// Provides bidirectional binding capabilities for components.
/// </summary>
/// <typeparam name="TValue">The type of value.</typeparam>
public interface IHasValueBound<TValue>:IBlazorComponent
{
    /// <summary>
    /// Gets or sets the value to bind.
    /// </summary>
    /// <example>
    /// @bind-Value="model.PropertyName"
    /// </example>
    TValue? Value { get; set; }
    /// <summary>
    /// Gets or sets the callback method that updates the binding value.
    /// </summary>
    EventCallback<TValue?> ValueChanged { get; set; }
}
