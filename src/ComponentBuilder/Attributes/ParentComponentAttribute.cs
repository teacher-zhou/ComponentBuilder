namespace ComponentBuilder;

/// <summary>
/// 应用于组件类，表示当前组件是父组件，子组件可以使用 <see cref="CascadingParameterAttribute"/> 获取父组件的实例。
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ParentComponentAttribute : Attribute
{
    /// <summary>
    /// 级联参数的名称。
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 获取或设置一个布尔值，表示级联参数的值是固定的。
    /// </summary>
    public bool IsFixed { get; set; }
}
