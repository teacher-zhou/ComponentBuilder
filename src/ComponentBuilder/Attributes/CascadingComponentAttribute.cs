namespace ComponentBuilder.Attributes;


/// <summary>
/// Indicates a cascading component of parameter that has <see cref="CascadingParameterAttribute"/> will be automtically added to the parent component.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class CascadingComponentAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="CascadingComponentAttribute"/> class.
    /// </summary>
    /// <param name="componentTypes">The parent component types.</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentTypes"/> is null.</exception>
    public CascadingComponentAttribute(params Type[] componentTypes)
    {
        ComponentTypes = componentTypes ?? throw new ArgumentNullException(nameof(componentTypes));
    }

    /// <summary>
    /// Gets the types of parent component.
    /// </summary>
    public Type[] ComponentTypes { get; }
}
