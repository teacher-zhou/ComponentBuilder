namespace ComponentBuilder;


/// <summary>
/// Represents a component is the child of specified <paramref name="componentType"/> component. And validate if this component is child of specified <paramref name="componentType"/> component in HTML layout, such as:
/// <code>
/// &lt;ParentComponent>
///     &lt;ChildComponent />
/// &lt;/ParentComponent>
/// 
/// &lt;ChildComponent /> //throw exception because is not child component
/// </code>
/// 
/// Getting the cascading value automatically for <paramref name="componentType"/> when has <see cref="CascadingParameterAttribute"/> for parameter with same type of <paramref name="componentType"/> witch is <c>public</c> modifier.
/// </summary>
/// <param name="componentType">The parent component type.</param>
/// <param name="optional">If <c>true</c> to indicate the child component do not perform nested checks without parent component.</param>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]

public class ChildComponentAttribute(Type componentType, bool optional = default) : Attribute
{
    /// <summary>
    /// Gets the type of parent component.
    /// </summary>
    public Type ComponentType => componentType;

    /// <summary>
    /// Gets or sets a <see cref="bool"/> value that indicates the child component do not perform nested checks without the parent component.
    /// </summary>
    public bool Optional => optional;
}

/// <summary>
/// Represents a component is the child of specified <typeparamref name="TParentComponent"/> component. And validate if this component is child of specified <typeparamref name="TParentComponent"/> component in HTML layout, such as:
/// <code>
/// &lt;ParentComponent>
///     &lt;ChildComponent />
/// &lt;/ParentComponent>
/// 
/// &lt;ChildComponent /> //throw exception because is not child component
/// </code>
/// 
/// Getting the cascading value automatically for <typeparamref name="TParentComponent"/> when has <see cref="CascadingParameterAttribute"/> for parameter with same type of <typeparamref name="TParentComponent"/> witch is <c>public</c> modifier.
/// </summary>
/// <param name="optional">If <c>true</c> to indicate the child component do not perform nested checks without parent component.</param>
public class ChildComponentAttribute<TParentComponent>(bool optional = default) : ChildComponentAttribute(typeof(TParentComponent), optional) where TParentComponent : IComponent { }