namespace ComponentBuilder;

/// <summary>
/// Indicates that the conditions for execution can be met.
/// </summary>
public readonly struct Condition
{
    readonly Func<bool>? _execution = default;
    readonly bool? _valid = default;

    /// <summary>
    /// Initializes a new instance of the <see cref="Condition"/> struct.
    /// </summary>
    /// <param name="valid"><c>true</c> to met condition.</param>
    public Condition(bool valid) => _valid = valid;

    /// <summary>
    /// Initializes a new instance of the <see cref="Condition"/> struct.
    /// </summary>
    /// <param name="execution">The operation is performed if conditions are met.</param>
    public Condition(Func<bool> execution) => _execution = execution;

    /// <summary>
    /// Gets the execution result if the conditions are met.
    /// </summary>
    public bool Result => _valid ?? _execution?.Invoke() ?? false;

    /// <summary>
    /// Perform the conditional action.
    /// </summary>
    /// <param name="condition">If <c>null</c> the condition returns <c>true</c>.</param>
    /// <param name="true">The operation to be performed when the condition is <c>true</c>.</param>
    /// <param name="false">The operation to be performed when the condition is <c>false</c>.</param>
    public static void Execute(Condition? condition, Action @true, Action? @false = default)
    {
        if ( Assert(condition) )
        {
            @true();
        }
        else
        {
            @false?.Invoke();
        }
    }

    /// <summary>
    /// Performs the conditional action and returns the result of the specified type.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="condition">If <c>null</c> the condition returns <c>true</c>.</param>
    /// <param name="true">The operation to be performed when the condition is <c>true</c>.</param>
    /// <param name="false">The operation to be performed when the condition is <c>false</c>.</param>
    public static TResult Execute<TResult>(Condition? condition, Func<TResult> @true, Func<TResult> @false) 
        => Assert(condition) ? @true() : @false();

    /// <summary>
    /// Asserts whether the specified condition is met.
    /// </summary>
    /// <param name="condition">If <c>null</c> the condition returns <c>true</c>.</param>
    /// <returns>If <paramref name="condition"/> is <c>null</c> or <see cref="Condition.Result "/> is <c>true</c>, return <c>true</c>, Otherwise, return <c>false</c>.</returns>
    public static bool Assert(Condition? condition)
        => condition is null || condition.Value.Result;

    /// <summary>
    /// Converts the <c>bool</c> value to the <see cref="Condition"/> class.
    /// </summary>
    /// <param name="value">The bool value.</param>
    public static implicit operator Condition(bool value) => new(value);
    /// <summary>
    /// Converts a function that returns a bool value to the <see cref="Condition"/> class.
    /// </summary>
    /// <param name="value">A function that returns a Boolean value.</param>
    public static implicit operator Condition(Func<bool> value) => new(value);
}
