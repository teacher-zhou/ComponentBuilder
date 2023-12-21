namespace ComponentBuilder;

/// <summary>
/// An abstract class that represents an enumeration pattern.
/// </summary>
public abstract class Enumeration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    protected Enumeration(string value) => Value = value;

    /// <summary>
    /// Gets the value of the enumeration.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Gets the enumeration member.
    /// </summary>
    /// <returns>An array of strings.</returns>
    public IEnumerable<string?> GetMembers()
        => GetType().GetFields(System.Reflection.BindingFlags.Static).Select(m => ((Enumeration?)m.GetValue(this))?.Value);
}
