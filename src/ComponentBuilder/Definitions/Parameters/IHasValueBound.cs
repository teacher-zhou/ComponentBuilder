namespace ComponentBuilder.Definitions;

/// <summary>
/// Provides bidirectional binding for components.
/// </summary>
/// <typeparam name="TValue">The value type.</typeparam>
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
    /// Gets or sets the callback method for updating the binding value.
    /// </summary>
    EventCallback<TValue?> ValueChanged { get; set; }
}
