using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ComponentBuilder;

/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> class.
/// </summary>
public static class RenderTreeBuilderExtensions
{
    const string CONDITION_ARGUMENT_WILL_BE_REMOVED = "condition 参数将在下一个版本移除，请改用 CreateElementIf 方法代替";

    #region CreateElement
    /// <summary>
    /// 使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    /// <returns>创建的元素引用或 <c>null</c>。</returns>
    [Obsolete(CONDITION_ARGUMENT_WILL_BE_REMOVED)]
    public static ElementReference? CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     RenderFragment content,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     bool condition = true,
                                     Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElement(sequence, elementName, (ContentRenderFragment)content, attributes, condition, builderFragmentAction);

    /// <summary>
    /// 使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    /// <returns>创建的元素引用或 <c>null</c>。</returns>
    [Obsolete(CONDITION_ARGUMENT_WILL_BE_REMOVED)]
    public static ElementReference? CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     ContentRenderFragment? content = default,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     bool condition = true,
                                     Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
    {
        if (string.IsNullOrEmpty(elementName))
        {
            throw new ArgumentException($"'{nameof(elementName)}' cannot be null or empty.", nameof(elementName));
        }

        ElementReference? element = default;

        if (!condition)
        {
            return element;
        }

        builder.OpenRegion(sequence);
        builder.OpenElement(0, elementName);

        int nextSequence = 1;

        var fragment = new RenderTreeBuilderFragment(builder, nextSequence);
        fragment.AddElementReferenceCapture(e => element = e);
        builderFragmentAction?.Invoke(fragment);
        nextSequence = fragment.LastSequence;

        if (attributes.HasValue)
        {
            builder.AddMultipleAttributes(nextSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes.Value));
        }

        if (content is not null)
        {
            builder.AddChildContent(nextSequence + 2, content);
        }

        builder.CloseElement();
        builder.CloseRegion();

        return element;
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时，使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">一个满足 <c>true</c> 时创建 HTML 元素的条件。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    public static ElementReference? CreateElementIf(this RenderTreeBuilder builder,
                                     bool condition,
                                     int sequence,
                                     string elementName,
                                     ContentRenderFragment? content = default,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
    {
        if (condition)
        {
            return builder.CreateElement(sequence, elementName, content, attributes, condition, builderFragmentAction);
        }
        return default;
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 返回 <c>true</c> 时，使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">执行一个可以返回布尔值条件的方法</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    public static ElementReference? CreateElementIf(this RenderTreeBuilder builder,
                                     [NotNull] Func<bool> condition,
                                     int sequence,
                                     string elementName,
                                     ContentRenderFragment? content = default,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElementIf(condition(), sequence, elementName, content, attributes, builderFragmentAction);

    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时，使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">一个满足 <c>true</c> 时创建 HTML 元素的条件。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    public static ElementReference? CreateElementIf(this RenderTreeBuilder builder,
                                     bool condition,
                                     int sequence,
                                     string elementName,
                                     RenderFragment content,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
    {
        if (condition)
        {
            builder.CreateElement(sequence, elementName, content, attributes, condition, builderFragmentAction);
        }
        return default;
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 返回 <c>true</c> 时，使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">执行一个可以返回布尔值条件的方法</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    public static ElementReference? CreateElementIf(this RenderTreeBuilder builder,
                                     Func<bool> condition,
                                     int sequence,
                                     string elementName,
                                     RenderFragment content,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateElementIf(condition(), sequence, elementName, content, attributes, builderFragmentAction);
    #endregion

    #region CreateComponent

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    [Obsolete(CONDITION_ARGUMENT_WILL_BE_REMOVED)]
    public static void CreateComponent(this RenderTreeBuilder builder,
                                       Type componentType,
                                       int sequence,
                                       RenderFragment content,
                                       OneOf<IReadOnlyDictionary<string, object>, object>? attributes,
                                       bool condition = true,
                                       Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateComponent(componentType, sequence, new ContentRenderFragment(content), attributes, condition, builderFragmentAction);

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    [Obsolete(CONDITION_ARGUMENT_WILL_BE_REMOVED)]
    public static void CreateComponent(this RenderTreeBuilder builder,
                                       Type componentType,
                                       int sequence,
                                       ContentRenderFragment? content = default,
                                       OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                       bool condition = true,
                                       Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
    {
        if (componentType is null)
        {
            throw new ArgumentNullException(nameof(componentType));
        }

        if (!condition)
        {
            return;
        }

        builder.OpenRegion(sequence);
        builder.OpenComponent(0, componentType);

        int nextSequence = 1;
        var fragment = new RenderTreeBuilderFragment(builder, nextSequence);
        builderFragmentAction?.Invoke(fragment);
        nextSequence = fragment.LastSequence;
        if (attributes is not null)
        {
            builder.AddMultipleAttributes(nextSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes.Value));
        }

        builder.AddChildContentAttribute(nextSequence + 2, content);

        builder.CloseComponent();
        builder.CloseRegion();
    }

    #endregion

    #region CreateComponentIf
    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">一个满足 <c>true</c> 时创建组件的条件。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public static void CreateComponentIf(this RenderTreeBuilder builder,
                                         bool condition,
                                         Type componentType,
                                         int sequence,
                                         ContentRenderFragment? content = default,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
    {
        if (condition)
        {
            builder.CreateComponent(componentType, sequence, content, attributes, condition, builderFragmentAction);
        }
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 返回 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">执行一个可以返回布尔值条件的方法</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public static void CreateComponentIf(this RenderTreeBuilder builder,
                                         Func<bool> condition,
                                         Type componentType,
                                         int sequence,
                                         ContentRenderFragment? content = default,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateComponentIf(condition(), componentType, sequence, content, attributes, builderFragmentAction);

    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">一个满足 <c>true</c> 时创建组件的条件。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public static void CreateComponentIf(this RenderTreeBuilder builder,
                                         bool condition,
                                         Type componentType,
                                         int sequence,
                                         RenderFragment content,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
    {
        if (condition)
        {
            builder.CreateComponent(componentType, sequence, content, attributes, condition, builderFragmentAction);
        }
    }
    /// <summary>
    /// 当 <paramref name="condition"/> 返回 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">执行一个可以返回布尔值条件的方法</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public static void CreateComponentIf(this RenderTreeBuilder builder,
                                         Func<bool> condition,
                                         Type componentType,
                                         int sequence,
                                         RenderFragment content,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        => builder.CreateComponentIf(condition(), componentType, sequence, content, attributes, builderFragmentAction);
    #endregion

    #region CreateComponent<TComponent>
    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">组件的 UI 片段。</param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    [Obsolete(CONDITION_ARGUMENT_WILL_BE_REMOVED)]
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   RenderFragment childContent,
                                                   OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                                   bool condition = true,
                                                   Action<RenderTreeBuilderFragment>? builderFragmentAction = default) where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, childContent, attributes, condition, builderFragmentAction);

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">组件的 UI 片段。</param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    [Obsolete(CONDITION_ARGUMENT_WILL_BE_REMOVED)]
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   ContentRenderFragment? childContent = default,
                                                   OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                                   bool condition = true,
                                                   Action<RenderTreeBuilderFragment>? builderFragmentAction = default) where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, childContent, attributes, condition, builderFragmentAction);


    #endregion


    #region CreateComponentIf<TComponent>
    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">一个满足 <c>true</c> 时创建组件的条件。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateComponentIf<TComponent>(this RenderTreeBuilder builder,
                                         bool condition,
                                         int sequence,
                                         ContentRenderFragment? content = default,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        where TComponent : ComponentBase
    {
        if (condition)
        {
            builder.CreateComponent<TComponent>(sequence, content, attributes, condition, builderFragmentAction);
        }
    }

    /// <summary>
    /// 当 <paramref name="condition"/> 返回 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">执行一个可以返回布尔值条件的方法</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateComponentIf<TComponent>(this RenderTreeBuilder builder,
                                         Func<bool> condition,
                                         int sequence,
                                         ContentRenderFragment? content = default,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        where TComponent : ComponentBase
        => builder.CreateComponentIf<TComponent>(condition(), sequence, content, attributes, builderFragmentAction);

    /// <summary>
    /// 当 <paramref name="condition"/> 为 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">一个满足 <c>true</c> 时创建组件的条件。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateComponentIf<TComponent>(this RenderTreeBuilder builder,
                                         bool condition,
                                         int sequence,
                                         RenderFragment content,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        where TComponent : ComponentBase
    {
        if (condition)
        {
            builder.CreateComponent<TComponent>(sequence, content, attributes, condition, builderFragmentAction);
        }
    }
    /// <summary>
    /// 当 <paramref name="condition"/> 返回 <c>true</c> 时创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="condition">执行一个可以返回布尔值条件的方法</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">组件可包含的 UI 片段，
    /// 确保组件具有 <c>[Parameter]public RenderFragment? ChildContent { get; set; }</c> 参数来创建子标记。
    /// </param>
    /// <param name="attributes">组件的参数。使用匿名类设置组件的参数，参数名称和数据类型要一致。
    /// <para>
    /// 参考示例：
    /// <code>
    /// new { 
    ///         Disabled = true, 
    ///         ChildContent = builder => builder.AddContent(0,"xxx"),
    ///         @class = "my-class",
    ///         style = "width:100px;color:red"
    ///         ...
    ///     } 
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builderFragmentAction">表示需要对当前渲染树进行的补充操作。</param>
    public static void CreateComponentIf<TComponent>(this RenderTreeBuilder builder,
                                         Func<bool> condition,
                                         int sequence,
                                         RenderFragment content,
                                         OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                         Action<RenderTreeBuilderFragment>? builderFragmentAction = default)
        where TComponent : ComponentBase
        => builder.CreateComponentIf<TComponent>(condition(), sequence, content, attributes, builderFragmentAction);
    #endregion

    #region CreateCascadingComponent
    /// <summary>
    /// 创建具备级联参数的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="value">级联参数的值。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">呈现此元素的UI内容的委托。</param>
    /// <param name="name">级联参数的名称。</param>
    /// <param name="isFixed">若为 <c>true</c>, 表示 <see cref="CascadingValue{TValue}.Value"/> 不会改变。这是一种性能优化，允许框架跳过设置更改通知。</param>
    /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> 或 <paramref name="content"/> 是 null.
    /// </exception>
    public static void CreateCascadingComponent<TValue>(this RenderTreeBuilder builder, TValue value, int sequence, [NotNull] RenderFragment content, string? name = default, bool isFixed = default)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        builder.OpenRegion(sequence);
        builder.OpenComponent<CascadingValue<TValue>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<TValue>.ChildContent), content);
        if (!string.IsNullOrEmpty(name))
        {
            builder.AddAttribute(2, nameof(CascadingValue<TValue>.Name), name);
        }
        builder.AddAttribute(3, nameof(CascadingValue<TValue>.IsFixed), isFixed);
        builder.AddAttribute(4, nameof(CascadingValue<TValue>.Value), value);
        builder.CloseComponent();
        builder.CloseRegion();
    }

    /// <summary>
    /// 创建具备级联参数的组件。
    /// </summary>
    /// <typeparam name="TValue">级联参数的值类型。</typeparam>
    /// <param name="component"></param>
    /// <param name="builder">The <see cref="ComponentBase"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">呈现此元素的UI内容的委托。</param>
    /// <param name="name">级联参数的名称。</param>
    /// <param name="isFixed">若为 <c>true</c>, 表示 <see cref="CascadingValue{TValue}.Value"/> 不会改变。这是一种性能优化，允许框架跳过设置更改通知。</param>
    /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> 或 <paramref name="content"/> 是 null.
    /// </exception>
    public static void CreateCascadingComponent<TValue>([NotNull] this ComponentBase component, RenderTreeBuilder builder, int sequence, [NotNull] RenderFragment content, string? name = default, bool isFixed = default)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (content is null)
        {
            throw new ArgumentNullException(nameof(content));
        }

        builder.OpenRegion(sequence);
        builder.OpenComponent<CascadingValue<TValue>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<TValue>.ChildContent), content);
        if (!string.IsNullOrEmpty(name))
        {
            builder.AddAttribute(2, nameof(CascadingValue<TValue>.Name), name);
        }
        builder.AddAttribute(3, nameof(CascadingValue<TValue>.IsFixed), isFixed);
        builder.AddAttribute(4, nameof(CascadingValue<TValue>.Value), component);
        builder.CloseComponent();
        builder.CloseRegion();
    }
    #endregion

    #region Style
    /// <summary>
    /// 添加样式的内容。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="selector">CSS 选择器字符串。</param>
    /// <param name="styleAttributes">选择器中的样式定义。
    /// <para>
    /// 使用匿名类型定义样式的键值对，示例如下：
    /// <code>
    /// new { width = "100px", height = "40px" ...}
    /// </code>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddStyleContent(this RenderTreeBuilder builder, int sequence, string selector, object styleAttributes)
    {
        if (string.IsNullOrEmpty(selector))
        {
            throw new ArgumentException($"'{nameof(selector)}' cannot be null or empty.", nameof(selector));
        }

        if (styleAttributes is null)
        {
            throw new ArgumentNullException(nameof(styleAttributes));
        }

        var styleBuilder = new StringBuilder($"{selector} {{\n");
        styleBuilder.AppendLine(BuildStyleAttributes(styleAttributes));
        styleBuilder.AppendLine("}");

        builder.AddContent(sequence, styleBuilder.ToString());

        static string BuildStyleAttributes(object keyValues)
        {
            return keyValues.GetType().GetProperties().Select(m => $"\t\t{m.Name.ToLower()}: {m.GetValue(keyValues)};").Aggregate((prev, next) => $"{prev}\n{next}");
        }
    }

    /// <summary>
    /// 创建一个具备自定义样式的区域，即 <c>&lt;style>...&lt;/style></c> 代码片段。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="selector">一个样式选择器的行为。</param>
    /// <param name="type">样式的类型。</param>
    public static void CreateStyles(this RenderTreeBuilder builder, int sequence, Action<StyleSelector> selector, string type = "text/css")
    {
        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        var creator = new StyleSelector();
        selector.Invoke(creator);
        builder.CreateElement(sequence, "style", creator.ToString(), new { type });
    }
    #endregion

    #region AddChildContent

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与 <see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">当前渲染树的子内容。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, RenderFragment? childContent)
    {
        return AddChildContentAttribute(builder, sequence, (ContentRenderFragment)childContent);
    }

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与 <see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">当前渲染树的子内容。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, ContentRenderFragment? childContent)
    {
        builder.AddAttribute(sequence, "ChildContent", (RenderFragment)(content =>
        {
            content.AddChildContent(0, childContent);
        }));
        return builder;
    }

    /// <summary>
    /// 追加下级内容到当前渲染树。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">当前渲染树的子内容。</param>
    public static void AddChildContent(this RenderTreeBuilder builder, int sequence, ContentRenderFragment? childContent)
        => childContent?.AddContent(builder, sequence);
    /// <summary>
    /// 追加下级内容到当前渲染树。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">当前渲染树的子内容。</param>
    public static void AddChildContent(this RenderTreeBuilder builder, int sequence, RenderFragment? childContent)
        => builder.AddChildContent(sequence, (ContentRenderFragment)childContent);
    #endregion

    #region AddClassAttribute
    /// <summary>
    /// Append attribute 'class' to this <see cref="RenderTreeBuilder"/> instance.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer reprisenting a squence of source code.</param>
    /// <param name="cssClasses">
    /// An array of value to add 'class' attribute. 
    /// <para>
    /// This value support a single string representing css class or the key/value paires represeting a condition is true to add given class string. 
    /// </para>
    /// <para>
    /// Here is example:
    /// </para>
    /// <code>
    /// builder.AddClassAttribute(sequence, "active", (isDisabeld, "is-disabled"), (Color.HasValue, "btn-primary")
    /// </code>
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="cssClasses"/> is null.</exception>
    public static void AddClassAttribute(this RenderTreeBuilder builder, int sequence, params OneOf<string?, (bool condition, string? css)>[] cssClasses)
    {
        if (cssClasses is null)
        {
            throw new ArgumentNullException(nameof(cssClasses));
        }

        var classList = new List<string>();
        foreach (var item in cssClasses)
        {
            item.Switch(value =>
            {
                if (!string.IsNullOrEmpty(value))
                {
                    classList.Add(value);
                }
            },
                value =>
                {
                    if (value.condition && !string.IsNullOrEmpty(value.css))
                    {
                        classList.Add(value.css);
                    }
                });
        }

        if (classList.Any())
        {
            builder.AddAttribute(sequence, "class", string.Join(" ", classList.Distinct()));
        }
    }
    #endregion

    #region AddStyleAttribute

    /// <summary>
    /// Append attribute 'class' to this <see cref="RenderTreeBuilder"/> instance.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer reprisenting a squence of source code.</param>
    /// <param name="styles">
    /// An array of value to add 'style' attribute. 
    /// <para>
    /// This value support a single string representing css class or the key/value paires represeting a condition is true to add given class string. 
    /// </para>
    /// <para>
    /// Here is example:
    /// </para>
    /// <code>
    /// builder.AddStyleAttribute(sequence, "height:100px", (Active, "display:block"), (Width.HasValue, $"width:{Width}px"))
    /// </code>
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="styles"/> is null.</exception>
    public static void AddStyleAttribute(this RenderTreeBuilder builder, int sequence, params OneOf<string?, (bool condition, string? style)>[] styles)
    {
        if (styles is null)
        {
            throw new ArgumentNullException(nameof(styles));
        }

        var styleList = new List<string>();
        foreach (var item in styles)
        {
            item.Switch(value =>
            {
                if (!string.IsNullOrEmpty(value))
                {
                    styleList.Add(value);
                }
            }
            , value =>
            {
                if (value.condition && !string.IsNullOrEmpty(value.style))
                {
                    styleList.Add(value.style);
                }
            });
        }

        if (styleList.Any())
        {
            builder.AddAttribute(sequence, "style", string.Join(";", styleList));
        }
    }
    #endregion


}
