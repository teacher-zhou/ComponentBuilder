namespace ComponentBuilder.Definitions;

/// <summary>
/// 提供组件支持表单和验证。
/// </summary>
[HtmlTag("form")]
public interface IFormComponent : IHasEditContext, IHasChildContent<EditContext>
{
    /// <summary>
    /// 指定表单的顶级模型对象。将为模型构造一个编辑上下文。
    /// </summary>
    object? Model { get; set; }
    /// <summary>
    /// 提交表单时将调用的回调。如果使用此参数，则由您手动触发任何验证，例如，通过调用 <see cref="EditContext.Validate" /> 方法。
    /// </summary>
    EventCallback<EditContext> OnSubmit { get; set; }
    /// <summary>
    ///  当提交表单时将调用回调函数，然后判断<see cref="EditContext"/>是否有效。
    /// </summary>
    EventCallback<EditContext> OnValidSubmit { get; set; }
    /// <summary>
    /// 当提交表单时将调用回调函数，然后<see cref="EditContext"/>被判断为无效。
    /// </summary>
    EventCallback<EditContext> OnInvalidSubmit { get; set; }

    /// <summary>
    /// 获取固定的编辑上下文。
    /// </summary>
    EditContext? FixedEditContext { get; set; }
}
