namespace ComponentBuilder;

/// <summary>
/// 应用于组件类。指示当前组件作为级联参数传入。
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
