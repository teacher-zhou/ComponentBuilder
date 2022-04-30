namespace ComponentBuilder.Attributes;
[AttributeUsage(AttributeTargets.Class)]
public class CascadingComponentAttribute : Attribute
{
    public CascadingComponentAttribute(params Type[] componentTypes)
    {
        ComponentTypes = componentTypes ?? throw new ArgumentNullException(nameof(componentTypes));
    }

    public Type[] ComponentTypes { get; }
}
