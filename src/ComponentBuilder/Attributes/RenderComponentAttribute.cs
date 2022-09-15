namespace ComponentBuilder;

/// <summary>
/// 应用于组件类。表示当前组件将使用指定类型的组件作为渲染，所有当前组件的定义都属于指定组件。
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class RenderComponentAttribute : Attribute
{
    /// <summary>
    /// 初始化 <see cref="RenderComponentAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="componentType">要渲染的组件类型。不能是当前组件，否则会引起死循环。</param>
    public RenderComponentAttribute(Type componentType)
    {
        ComponentType = componentType;
    }

    /// <summary>
    /// 获取组件类型。
    /// </summary>
    public Type ComponentType { get; }
}
