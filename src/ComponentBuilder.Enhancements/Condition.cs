namespace ComponentBuilder.Enhancement;

/// <summary>
/// A condition that can satisifed to execute.
/// </summary>
public struct Condition
{
    readonly Func<bool>? _execution = default;
    readonly bool? _valid = default;

    /// <summary>
    /// Initializes a new instance of the <see cref="Condition"/> class.
    /// </summary>
    /// <param name="valid">The valid result.</param>
    public Condition(bool valid) => _valid = valid;

    /// <summary>
    /// Initializes a new instance of the <see cref="Condition"/> class.
    /// </summary>
    /// <param name="execution">The function to execute.</param>
    public Condition(Func<bool> execution) => _execution = execution;

    /// <summary>
    /// Gets the result of condition.
    /// </summary>
    public bool Result => _valid ?? _execution?.Invoke() ?? false;

    /// <summary>
    /// Execute the action of condition.
    /// </summary>
    /// <param name="condition">The condition that can be null to be <c>true</c>.</param>
    /// <param name="true">The action when condition returns <c>true</c>.</param>
    /// <param name="false">The action when condition returns <c>false</c>.</param>
    public static void Execute(Condition? condition, Action @true, Action? @false=default)
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
    /// Execute the action of condition with return value.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="condition">The condition that can be null to be <c>true</c>.</param>
    /// <param name="true">The action when condition returns <c>true</c>.</param>
    /// <param name="false">The action when condition returns <c>false</c>.</param>
    public static TResult Execute<TResult>(Condition? condition, Func<TResult> @true, Func<TResult> @false) 
        => Assert(condition) ? @true() : @false();

    /// <summary>
    /// Assert specified condition is <c>true</c>.
    /// </summary>
    /// <param name="condition">The condition that can be null to be <c>true</c>.</param>
    /// <returns><c>True</c> when <paramref name="condition"/> is null or <see cref="Condition.Result"/> is <c>true</c>; otherwise,<c>false</c>.</returns>
    public static bool Assert(Condition? condition)
        => condition is null || condition.Value.Result;

    /// <summary>
    /// Converts <c>bool</c> value to <see cref="Condition"/> class.
    /// </summary>
    /// <param name="value">The bool value.</param>
    public static implicit operator Condition(bool value) => new(value);
    /// <summary>
    /// Converts a function that returns a bool value to <see cref="Condition"/> class.
    /// </summary>
    /// <param name="value">The function returns bool value.</param>
    public static implicit operator Condition(Func<bool> value) => new(value);
}
