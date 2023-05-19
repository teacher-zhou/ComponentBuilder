namespace ComponentBuilder;

/// <summary>
/// 表示组件是一个父组件，并自动创建该组件的一个级联参数。会与标记了 <see cref="ChildComponentAttribute"/> 的子组件进行关联校验。
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ParentComponentAttribute : Attribute
{
    /// <summary>
    /// 获取或设置级联参数的名称。
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 获取或设置一个布尔值，该值指示级联参数的值是固定的。
    /// </summary>
    public bool IsFixed { get; set; }
}
