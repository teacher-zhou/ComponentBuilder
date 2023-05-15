namespace ComponentBuilder.Automation;

/// <summary>
/// Represents component class is parent class and create cascade parameter for this component.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ParentComponentAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of cascade parameter.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// Gets or sets a Boolean value indicating that the value of the cascade parameter is fixed.
    /// </summary>
    public bool IsFixed { get; set; }
}
