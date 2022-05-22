namespace ComponentBuilder;

/// <summary>
/// 应用于组件的参数类型是 <see cref="EventCallback"/> 或 <see cref="EventCallback{TValue}"/> 事件的参数。
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class HtmlEventAttribute : HtmlAttributeAttribute
{
    /// <summary>
    /// 使用指定的事件名称初始化 <see cref="HtmlEventAttribute"/> 类的新实例。
    /// </summary>
    /// <param name="name">HTML 事件名称。例如 <c>onclick</c> 。</param>
    public HtmlEventAttribute(string name) : base(name)
    {
    }
}
