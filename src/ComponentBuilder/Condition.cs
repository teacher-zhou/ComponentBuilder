namespace ComponentBuilder;

/// <summary>
/// A condition that can satisifed to execute.
/// </summary>
public class Condition : OneOfBase<bool, Func<bool>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Condition"/> class.
    /// </summary>
    /// <param name="input">The input.</param>
    protected Condition(OneOf<bool, Func<bool>> input) : base(input)
    {
    }

    /// <summary>
    /// Gets the result of condition.
    /// </summary>
    public bool Result => (bool)Value;

    /// <summary>
    /// Execute the action of condition.
    /// </summary>
    /// <param name="condition">The condition that can be null to be <c>true</c>.</param>
    /// <param name="true">The action when condition returns <c>true</c>.</param>
    /// <param name="false">The action when condition returns <c>false</c>.</param>
    public static void Execute(Condition? condition, Action @true, Action? @false=default)
    {
        if ( condition is null || condition.Result )
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
    {
        if ( condition is null || condition.Result )
        {
            return @true();
        }
        return @false();
    }

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
