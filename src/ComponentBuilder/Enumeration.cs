namespace ComponentBuilder;

/// <summary>
/// Represents an <see langword="abstract"/> class for enum pattern.
/// </summary>
public abstract class Enumeration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration"/> class.
    /// </summary>
    /// <param name="value">The value of enum.</param>
    protected Enumeration(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the members.
    /// </summary>
    /// <returns>A list of string?.</returns>
    public IEnumerable<string?> GetMembers()
        => GetType().GetFields(System.Reflection.BindingFlags.Static).Select(m => ((Enumeration?)m.GetValue(this))?.Value);
}
