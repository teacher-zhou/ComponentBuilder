namespace ComponentBuilder;

/// <summary>
/// Represents component is parent component, and automatically creating a cascading parameter for this component.
/// </summary>
/// <param name="name">The name of cascading value.</param>
[AttributeUsage(AttributeTargets.Class)]
public class ParentComponentAttribute(string? name = default) : Attribute
{
    /// <summary>
    /// Gets the name of cascading value.
    /// </summary>
    public string? Name => name;
    /// <summary>
    /// Gets or sets a <see cref="bool"/> vlaue wheither the value of cascading is fixed.
    /// </summary>
    public bool IsFixed { get; set; }
}
