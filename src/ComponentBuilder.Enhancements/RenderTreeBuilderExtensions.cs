using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> class.
/// </summary>
public static class RenderTreeBuilderExtensions
{
    #region CreateElement
    /// <summary>
    /// 创建具有指定元素名称的HTML元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的标记内容。</param>
    /// <param name="attributes">HTML 元素的属性。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是 null 或空字符串。</exception>
    public static void CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     MarkupString? content,
                                     object? attributes = default,
                                     Condition? condition = default,
                                     object? key = default,
                                     Action<ElementReference>? captureReference = default)
        => builder.CreateElement(sequence, elementName, b => b.AddContent(0, content), attributes, condition, key, captureReference);

    /// <summary>
    /// 创建具有指定元素名称的HTML元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的文本内容。</param>
    /// <param name="attributes">HTML 元素的属性。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是 null 或空字符串。</exception>
    public static void CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     string? content,
                                     object? attributes = default,
                                     Condition? condition = default,
                                     object? key = default,
                                     Action<ElementReference>? captureReference = default)
        => builder.CreateElement(sequence, elementName, b => b.AddContent(0, content), attributes, condition, key, captureReference);


    /// <summary>
    /// 创建具有指定元素名称的HTML元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的任意内容片段。</param>
    /// <param name="attributes">HTML 元素的属性。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是 null 或空字符串。</exception>
    public static void CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     RenderFragment? content = default,
                                     object? attributes = default,
                                     Condition? condition = default,
                                     object? key = default,
                                     Action<ElementReference>? captureReference = default)
    {
        if (string.IsNullOrEmpty(elementName))
        {
            throw new ArgumentException($"'{nameof(elementName)}' cannot be null or empty.", nameof(elementName));
        }

        if (!Condition.Assert(condition))
        {
            return;
        }

        builder.OpenRegion(sequence);
        builder.OpenElement(0, elementName);

        builder.SetKey(key);

        int nextSequence = 1;

        if (attributes is not null)
        {
            builder.AddMultipleAttributes(nextSequence + 1, HtmlHelper.Instance.MergeHtmlAttributes(attributes));
        }

        if ( captureReference is not null)
        {
            builder.AddElementReferenceCapture(nextSequence + 2, captureReference);
        }

        builder.AddContent(nextSequence + 3, content);

        builder.CloseElement();
        builder.CloseRegion();
    }

    #endregion

    #region CreateComponent
    /// <summary>
    /// 创建具有指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="content">组件的标记内容。</param>
    /// <param name="attributes">组件的参数。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> 是 null。</exception>
    public static void CreateComponent(this RenderTreeBuilder builder,
                                          Type componentType,
                                          int sequence,
                                          MarkupString? content,
                                          object? attributes = default,
                                          Condition? condition = default,
                                          object? key = default,
                                          Action<object>? captureReference = default)
        => builder.CreateComponent(componentType, sequence, b => b.AddContent(0, content), attributes, condition, key, captureReference);

    /// <summary>
    /// 创建具有指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="content">组件的文本内容。</param>
    /// <param name="attributes">组件的参数。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> 是 null。</exception>
    public static void CreateComponent(this RenderTreeBuilder builder,
                                          Type componentType,
                                          int sequence,
                                          string? content,
                                          object? attributes = default,
                                          Condition? condition = default,
                                          object? key = default,
                                          Action<object>? captureReference = default)
        => builder.CreateComponent(componentType, sequence, b => b.AddContent(0, content), attributes, condition, key, captureReference);


    /// <summary>
    /// 创建具有指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="content">组件的任意内容片段。</param>
    /// <param name="attributes">组件的参数。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> 是 null。</exception>
    public static void CreateComponent(this RenderTreeBuilder builder,
                                          Type componentType,
                                          int sequence,
                                          RenderFragment? content = default,
                                          object? attributes = default,
                                          Condition? condition = default,
                                          object? key = default,
                                          Action<object>? captureReference = default)
    {
        if (componentType is null)
        {
            throw new ArgumentNullException(nameof(componentType));
        }

        if (!Condition.Assert(condition))
        {
            return;
        }

        builder.OpenRegion(sequence);
        builder.OpenComponent(0, componentType);
        builder.SetKey(key);

        int nextSequence = 1;
        if (attributes is not null)
        {
            builder.AddMultipleAttributes(nextSequence + 1, HtmlHelper.Instance.MergeHtmlAttributes(attributes));
        }

        if ( componentType.GetProperty("ChildContent") is not null )
        {
            builder.AddAttribute(nextSequence + 2, "ChildContent", content);
        }

        if ( captureReference is not null )
        {
            builder.AddComponentReferenceCapture(nextSequence + 3, captureReference);
        }
        builder.CloseComponent();
        builder.CloseRegion();
    }

    #endregion

    #region CreateComponent<TComponent>

    /// <summary>
    /// 创建具有指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="content">组件的标记内容。</param>
    /// <param name="attributes">组件的参数。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   MarkupString? content,
                                                   object? attributes = default,
                                                   Condition? condition = default,
                                                   object? key = default,
                                                   Action<object>? captureReference = default)
        where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, content, attributes, condition, key, captureReference);

    /// <summary>
    /// 创建具有指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="content">组件的文本内容。</param>
    /// <param name="attributes">组件的参数。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   string? content,
                                                   object? attributes = default,
                                                   Condition? condition = default,
                                                   object? key = default,
                                                   Action<object>? captureReference = default)
        where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, content, attributes, condition, key, captureReference);

    /// <summary>
    /// 创建具有指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">指令在源代码中的位置的整数。</param>
    /// <param name="content">组件的任意内容片段。</param>
    /// <param name="attributes">组件的参数。
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </param>
    /// <param name="condition">满足创建 HTML 元素的条件。</param>
    /// <param name="key">指定此元素的 key 值。</param>
    /// <param name="captureReference">当参考值更改时调用的操作。</param>
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   RenderFragment? content = default,
                                                   object? attributes = default,
                                                   Condition? condition = default,
                                                   object? key = default,
                                                   Action<object>? captureReference = default)
        where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, content, attributes, condition, key, captureReference);


    #endregion

    #region CreateCascadingComponent
    /// <summary>
    /// 创建指定值的级联组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create.</param>
    /// <param name="value">级联参数的值。</param>
    /// <param name="sequence">指示指令在源代码中的位置的整数。</param>
    /// <param name="content">呈现此元素的UI内容的委托。</param>
    /// <param name="name">级联参数名称。</param>
    /// <param name="isFixed">如果为 <c>true</c>，则<see cref="CascadingValue{TValue}.Value"/> 的值不可修改。这是一个性能优化，允许框架跳过设置更改通知。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> or <paramref name="content"/> 是 null。
    /// </exception>
    public static void CreateCascadingComponent<TValue>(this RenderTreeBuilder builder,
                                                        TValue value,
                                                        int sequence,
                                                        [NotNull] RenderFragment content,
                                                        string? name = default,
                                                        bool isFixed = default)
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
    #endregion

    #region Style

    /// <summary>
    /// 创建一个样式区域，如 <c>&lt;style>...&lt;/style></c>。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">指示指令在源代码中的位置的整数。</param>
    /// <param name="selector">用于创建样式的选择器。</param>
    /// <param name="type">样式类型。</param>
    public static void CreateStyleRegion(this RenderTreeBuilder builder, int sequence, Action<StyleSelector> selector, string type = "text/css")
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

}
