namespace ComponentBuilder.Attributes;

/// <summary>
/// Provides a specific component type to render.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RenderCompoentAttribute : Attribute
{
    public RenderCompoentAttribute(Type componentType)
    {
        ComponentType = componentType;
    }

    public Type ComponentType { get; }
}
