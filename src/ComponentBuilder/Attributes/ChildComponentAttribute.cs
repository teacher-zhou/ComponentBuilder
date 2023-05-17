namespace ComponentBuilder;


/// <summary>
/// Applies to component classes. Indicates that the current component is a child of the specified component and does association validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ChildComponentAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildComponentAttribute"/> class.
    /// </summary>
    /// <param name="componentType">The component to associate.</param>
    public ChildComponentAttribute(Type componentType)
    {
        ComponentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
    }

    /// <summary>
    /// The type associated component.
    /// </summary>
    public Type ComponentType { get; }

    /// <summary>
    /// Gets or sets a Boolean value indicating that the parent component is optional and does not enforce association validation.
    /// </summary>
    public bool Optional { get; set; }
}

#if NET7_0_OR_GREATER

/// <summary>
/// Applies to component classes. Indicates that the current component is a child of the specified component and does association validation.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ChildComponentAttribute<TComponent>: ChildComponentAttribute where TComponent:IComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChildComponentAttribute{TComponent}"/> class.
    /// </summary>
    public ChildComponentAttribute():base(typeof(TComponent))
    {
    }
}
#endif