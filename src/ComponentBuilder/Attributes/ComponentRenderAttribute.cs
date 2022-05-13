namespace ComponentBuilder;

/// <summary>
/// Provides a specific component type to render.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ComponentRenderAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of <see cref="ComponentRenderAttribute"/> class with speicific type of component.
    /// </summary>
    /// <param name="componentType">The component type of create.</param>
    public ComponentRenderAttribute(Type componentType)
    {
        ComponentType = componentType;
    }

    /// <summary>
    /// Gets the type of component.
    /// </summary>
    public Type ComponentType { get; }
}
