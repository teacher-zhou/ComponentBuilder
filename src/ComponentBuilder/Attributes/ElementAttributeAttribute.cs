namespace ComponentBuilder;

/// <summary>
/// Represents an element attribute in component that applying by parameter value.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ElementAttributeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ElementAttributeAttribute"/> class.
    /// </summary>
    public ElementAttributeAttribute() : this(null)
    {

    }
    /// <summary>
    /// Initializes a new instance of <see cref="ElementAttributeAttribute"/> class.
    /// </summary>
    /// <param name="name">The name of attribute for element.</param>
    public ElementAttributeAttribute(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Gets the name of atrribute.
    /// </summary>
    public string Name { get; }
}
