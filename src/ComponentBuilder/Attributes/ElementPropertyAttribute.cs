namespace ComponentBuilder.Attributes;

/// <summary>
/// Represents a element attribute in component that applying by parameter value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ElementPropertyAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ElementPropertyAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of attribute for element.</param>
    public ElementPropertyAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of atrribute.
    /// </summary>
    public string Name { get; }
}
