namespace ComponentBuilder;

/// <summary>
/// 应用于组件类。指示该组件渲染指定类型的组件。
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ComponentRenderAttribute : Attribute
{
    /// <summary>
    /// 初始化 <see cref="ComponentRenderAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="componentType">要渲染的组件类型。不能是当前组件，否则会引起死循环。</param>
    public ComponentRenderAttribute(Type componentType)
    {
        ComponentType = componentType;
    }

    /// <summary>
    /// 获取组件类型。
    /// </summary>
    public Type ComponentType { get; }
}
