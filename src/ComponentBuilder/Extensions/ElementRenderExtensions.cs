using OneOf;

namespace ComponentBuilder;

/// <summary>
/// 扩展 <see cref="RenderTreeBuilder"/> 创建元素。
/// </summary>
public static class ElementRenderExtensions
{
    /// <summary>
    /// 创建元素名称为 div 的 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateDiv(this RenderTreeBuilder builder,
                                 int sequence,
                                 ContentRenderFragment? content = default,
                                 OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                 bool condition = true,
                                 Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElementIf(condition, sequence, "div", content, attributes, builderFragmentAction);

    /// <summary>
    /// 创建元素名称为 div 的 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateDiv(this RenderTreeBuilder builder,
                                 int sequence,
                                 RenderFragment content,
                                 OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                 bool condition = true,
                                 Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElementIf(condition, sequence, "div", new ContentRenderFragment(content), attributes, builderFragmentAction);

    /// <summary>
    /// 创建元素名称为 span 的 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateSpan(this RenderTreeBuilder builder,
                                  int sequence,
                                  ContentRenderFragment? content = default,
                                  OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                  bool condition = true,
                                  Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElement(sequence, "span", content, attributes, condition, builderFragmentAction);

    /// <summary>
    /// 创建元素名称为 span 的 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateSpan(this RenderTreeBuilder builder,
                                  int sequence,
                                 RenderFragment content,
                                  OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                  bool condition = true,
                                  Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElement(sequence, "span", new ContentRenderFragment(content), attributes, condition, builderFragmentAction);
}
