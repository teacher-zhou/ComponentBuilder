namespace ComponentBuilder;

/// <summary>
/// Defines component can be parent and create cascading value of current component.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class NestedComponentAttribute : Attribute
{
    /// <summary>
    /// The name of cascading parameter.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Indicates a bool value weither cascading value is fixed.
    /// </summary>
    public bool IsFixed { get; set; }
}
