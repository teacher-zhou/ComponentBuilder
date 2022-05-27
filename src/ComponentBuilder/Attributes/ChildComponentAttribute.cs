namespace ComponentBuilder;


/// <summary>
/// 应用于组件类。指示当前组件是指定组件的子组件，并进行关联验证。
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ChildComponentAttribute : Attribute
{
    /// <summary>
    /// 初始化 <see cref="ChildComponentAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="componentType">父组件的类型。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public ChildComponentAttribute(Type componentType)
    {
        ComponentType = componentType ?? throw new ArgumentNullException(nameof(componentType));
    }

    /// <summary>
    /// 获取父组件类型。
    /// </summary>
    public Type ComponentType { get; }

    /// <summary>
    /// 获取或设置一个布尔值，表示父组件是可选的，不进行强制关联验证。
    /// </summary>
    public bool Optional { get; set; }
}
