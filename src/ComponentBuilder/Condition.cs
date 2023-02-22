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
