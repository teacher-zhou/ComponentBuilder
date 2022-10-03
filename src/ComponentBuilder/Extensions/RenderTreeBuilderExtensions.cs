using System.Diagnostics.CodeAnalysis;
using System.Text;

using OneOf;

namespace ComponentBuilder;

/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> class.
/// </summary>
public static class RenderTreeBuilderExtensions
{
    #region CreateElement
    /// <summary>
    /// 使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="childContent">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="appendFunc">用于追加自定义框架的函数委托。
    /// <para>
    /// <list type="bullet">
    /// <item>第一个参数：<see cref="RenderTreeBuilder"/> 的实例。</item>
    /// <item>第二个参数：当前源代码中最新的位置序列。</item>
    /// <item>返回值：任何操作过后最新的源代码位置序列。</item>
    /// </list>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    public static void CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     RenderFragment childContent,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     bool condition = true,
                                     Func<RenderTreeBuilder, int, int>? appendFunc = default)
        => builder.CreateElement(sequence, elementName, OneOf<string?, RenderFragment?, MarkupString?>.FromT1(childContent), attributes, condition, appendFunc);

    /// <summary>
    /// 使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="childContent">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。
    /// 可使用匿名类，<code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code></param>
    /// <param name="condition">当条件时 <c>true</c> 时创建。</param>
    /// <param name="appendFunc">用于追加自定义框架的函数委托。
    /// <para>
    /// <list type="bullet">
    /// <item>第一个参数：<see cref="RenderTreeBuilder"/> 的实例。</item>
    /// <item>第二个参数：当前源代码中最新的位置序列。</item>
    /// <item>返回值：任何操作过后最新的源代码位置序列。</item>
    /// </list>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> 是空字符串或 <c>null</c> 。</exception>
    public static void CreateElement(this RenderTreeBuilder builder,
                                     int sequence,
                                     string elementName,
                                     OneOf<string?, RenderFragment?, MarkupString?>? childContent = default,
                                     OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                     bool condition = true,
                                     Func<RenderTreeBuilder, int, int>? appendFunc = default)
    {
        if (string.IsNullOrEmpty(elementName))
        {
            throw new ArgumentException($"'{nameof(elementName)}' cannot be null or empty.", nameof(elementName));
        }

        if (!condition)
        {
            return;
        }

        builder.OpenRegion(sequence);
        builder.OpenElement(0, elementName);

        int lastSequence = 0;
        if (appendFunc is not null)
        {
            lastSequence = appendFunc.Invoke(builder, lastSequence);
        }

        if (attributes.HasValue)
        {
            builder.AddMultipleAttributes(lastSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes.Value));
        }

        if (childContent.HasValue)
        {
            builder.AddChildContent(lastSequence + 2, childContent.Value);
        }

        builder.CloseElement();
        builder.CloseRegion();
    }
    #endregion

    #region CreateComponent

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">组件可包含的 UI 片段，
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
    /// <param name="appendFunc">用于追加自定义框架的函数委托。
    /// <para>
    /// <list type="bullet">
    /// <item>第一个参数：<see cref="RenderTreeBuilder"/> 的实例。</item>
    /// <item>第二个参数：当前源代码中最新的位置序列。</item>
    /// <item>返回值：任何操作过后最新的源代码位置序列。</item>
    /// </list>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public static void CreateComponent(this RenderTreeBuilder builder,
                                       Type componentType,
                                       int sequence,
                                       RenderFragment childContent,
                                       OneOf<IReadOnlyDictionary<string, object>, object>? attributes,
                                       bool condition = true,
                                       Func<RenderTreeBuilder, int, int>? appendFunc = default)
        => builder.CreateComponent(componentType, sequence, OneOf<string?, RenderFragment?, MarkupString?>.FromT1(childContent), attributes, condition, appendFunc);

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">组件可包含的 UI 片段，
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
    /// <param name="appendFunc">用于追加自定义框架的函数委托。
    /// <para>
    /// <list type="bullet">
    /// <item>第一个参数：<see cref="RenderTreeBuilder"/> 的实例。</item>
    /// <item>第二个参数：当前源代码中最新的位置序列。</item>
    /// <item>返回值：任何操作过后最新的源代码位置序列。</item>
    /// </list>
    /// </para>
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="componentType"/> 是 null.</exception>
    public static void CreateComponent(this RenderTreeBuilder builder,
                                       Type componentType,
                                       int sequence,
                                       OneOf<string?, RenderFragment?, MarkupString?>? childContent = default,
                                       OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                       bool condition = true,
                                       Func<RenderTreeBuilder, int, int>? appendFunc = default)
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

        int lastSequence = 1;
        if (appendFunc is not null)
        {
            lastSequence = appendFunc.Invoke(builder, lastSequence);
        }

        if (attributes is not null)
        {
            builder.AddMultipleAttributes(lastSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes.Value));
        }

        if (childContent.HasValue)
        {
            builder.AddChildContentAttribute(lastSequence + 2, childContent.Value);
        }

        builder.CloseComponent();
        builder.CloseRegion();
    }

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件类型。</typeparam>
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
    /// <param name="appendFunc">用于追加自定义框架的函数委托。
    /// <para>
    /// <list type="bullet">
    /// <item>第一个参数：<see cref="RenderTreeBuilder"/> 的实例。</item>
    /// <item>第二个参数：当前源代码中最新的位置序列。</item>
    /// <item>返回值：任何操作过后最新的源代码位置序列。</item>
    /// </list>
    /// </para>
    /// </param>
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   RenderFragment childContent,
                                                   OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                                   bool condition = true,
                                                   Func<RenderTreeBuilder, int, int>? appendFunc = default) where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, childContent, attributes, condition, appendFunc);

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件类型。</typeparam>
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
    /// <param name="appendFunc">用于追加自定义框架的函数委托。
    /// <para>
    /// <list type="bullet">
    /// <item>第一个参数：<see cref="RenderTreeBuilder"/> 的实例。</item>
    /// <item>第二个参数：当前源代码中最新的位置序列。</item>
    /// <item>返回值：任何操作过后最新的源代码位置序列。</item>
    /// </list>
    /// </para>
    /// </param>
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder,
                                                   int sequence,
                                                   OneOf<string?, RenderFragment?, MarkupString?>? childContent = default,
                                                   OneOf<IReadOnlyDictionary<string, object>, object>? attributes = default,
                                                   bool condition = true,
                                                   Func<RenderTreeBuilder, int, int>? appendFunc = default) where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, childContent, attributes, condition, appendFunc);
    #endregion

    #region CreateCascadingComponent
    /// <summary>
    /// 创建具备级联参数的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="value">级联参数的值。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">呈现此元素的UI内容的委托。</param>
    /// <param name="name">级联参数的名称。</param>
    /// <param name="isFixed">若为 <c>true</c>, 表示 <see cref="CascadingValue{TValue}.Value"/> 不会改变。这是一种性能优化，允许框架跳过设置更改通知。</param>
    /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> 或 <paramref name="childContent"/> 是 null.
    /// </exception>
    public static void CreateCascadingComponent<TValue>(this RenderTreeBuilder builder, TValue value, int sequence, [NotNull] RenderFragment childContent, string? name = default, bool isFixed = default)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (childContent is null)
        {
            throw new ArgumentNullException(nameof(childContent));
        }

        builder.OpenRegion(sequence);
        builder.OpenComponent<CascadingValue<TValue>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<TValue>.ChildContent), childContent);
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
    /// <param name="childContent">呈现此元素的UI内容的委托。</param>
    /// <param name="name">级联参数的名称。</param>
    /// <param name="isFixed">若为 <c>true</c>, 表示 <see cref="CascadingValue{TValue}.Value"/> 不会改变。这是一种性能优化，允许框架跳过设置更改通知。</param>
    /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> 或 <paramref name="childContent"/> 是 null.
    /// </exception>
    public static void CreateCascadingComponent<TValue>([NotNull] this ComponentBase component, RenderTreeBuilder builder, int sequence, [NotNull] RenderFragment childContent, string? name = default, bool isFixed = default)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (childContent is null)
        {
            throw new ArgumentNullException(nameof(childContent));
        }

        builder.OpenRegion(sequence);
        builder.OpenComponent<CascadingValue<TValue>>(0);
        builder.AddAttribute(1, nameof(CascadingValue<TValue>.ChildContent), childContent);
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
    /// <param name="content">当前渲染树的子内容。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, RenderFragment? content)
        => builder.AddChildContentAttribute(sequence, OneOf<string?, RenderFragment?, MarkupString?>.FromT1(content));

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与 <see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="childContent">框架的内容。是 <see cref="string"/> 或 <see cref="RenderFragment"/> 类型。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, OneOf<string?, RenderFragment?, MarkupString?> childContent)
    {
        builder.AddAttribute(sequence, "ChildContent", (RenderFragment)(content =>
        {
            content.AddChildContent(0, childContent);
        }));
        return builder;
    }


    /// <summary>
    /// 追加子内容到当前渲染树。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">当前渲染树的子内容。</param>
    public static RenderTreeBuilder AddChildContent(this RenderTreeBuilder builder, int sequence, RenderFragment content)
        => builder.AddChildContent(sequence, OneOf<string?, RenderFragment?, MarkupString?>.FromT1(content));

    /// <summary>
    /// 追加下级内容到当前渲染树。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">当前渲染树的子内容。</param>
    public static RenderTreeBuilder AddChildContent(this RenderTreeBuilder builder, int sequence, OneOf<string?, RenderFragment?, MarkupString?> content)
    {
        content.Switch(
                str => builder.AddContent(sequence, str ?? string.Empty),
                fragment => builder.AddContent(sequence, fragment),
                markupString => builder.AddContent(sequence, markupString)
                );
        return builder;
    }
    #endregion
}
