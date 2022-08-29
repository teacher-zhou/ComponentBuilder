﻿using System.Diagnostics.CodeAnalysis;

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
    /// <param name="childContent">呈现此元素的UI内容的委托。</param>
    /// <param name="attributes">元素的 HTML 属性。</param>
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
    public static void CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, RenderFragment? childContent = default, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default)
    => builder.CreateElement(sequence, elementName, (object?)childContent, attributes, condition, appendFunc);

    /// <summary>
    /// 使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="markupString">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。</param>
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
    public static void CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, string? markupString, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default)
    => builder.CreateElement(sequence, elementName, (object?)markupString, attributes, condition, appendFunc);

    /// <summary>
    /// 使用指定的元素名称创建 HTML 元素。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="elementName">HTML 元素名称。</param>
    /// <param name="content">元素的 UI 片段。</param>
    /// <param name="attributes">元素的 HTML 属性。</param>
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
    internal static void CreateElement(this RenderTreeBuilder builder, int sequence, string elementName, object? content
        , object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default)
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

        if (attributes is not null)
        {
            builder.AddMultipleAttributes(lastSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes));
        }

        builder.AddChildContent(lastSequence + 2, content ?? string.Empty);


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
    /// <param name="childContent">呈现此元素的UI内容的委托。</param>
    /// <param name="attributes">组件的参数。</param>
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
    public static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, RenderFragment? childContent = default, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default)
    => builder.CreateComponent(componentType, sequence, (object?)childContent, attributes, condition, appendFunc);


    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="markupString">标记文本框架的内容。
    /// <para>
    /// 确保组件具有 ChildContent 参数来创建子标记字符串。
    /// </para> 
    /// </param>
    /// <param name="attributes">组件的参数。</param>
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
    public static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, string markupString, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default)
    => builder.CreateComponent(componentType, sequence, (object)markupString, attributes, condition, appendFunc);

    internal static void CreateComponent(this RenderTreeBuilder builder, Type componentType, int sequence, object? content, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default)
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
            builder.AddMultipleAttributes(lastSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes));
        }
        builder.AddChildContentAttribute(lastSequence + 2, content);

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
    /// <param name="attributes">组件的参数。</param>
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
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder, int sequence, RenderFragment? childContent = default, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default) where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, childContent, attributes, condition, appendFunc);

    /// <summary>
    /// 创建指定组件类型的组件。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="markupString">标记文本框架的内容。
    /// <para>
    /// 确保组件具有 ChildContent 参数来创建子标记字符串。
    /// </para> 
    /// </param>
    /// <param name="attributes">组件的参数。</param>
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
    public static void CreateComponent<TComponent>(this RenderTreeBuilder builder, int sequence, string markupString, object? attributes = default, bool condition = true, Func<RenderTreeBuilder, int, int>? appendFunc = default) where TComponent : ComponentBase
    => builder.CreateComponent(typeof(TComponent), sequence, markupString, attributes, condition, appendFunc);
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

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与<see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="textContent">新文本框架的内容。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, string textContent)
    => builder.AddChildContentAttribute(sequence, (object)textContent);

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与<see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="fragment">新片段框架的内容。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, RenderFragment fragment)
    => builder.AddChildContentAttribute(sequence, (object)fragment);

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与<see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="markupContent">为新的标记框架标记内容。</param>
    public static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, MarkupString markupContent)
    => builder.AddChildContentAttribute(sequence, (object)markupContent);

    /// <summary>
    /// 将文本追加到 ChildContent 参数。
    /// <para>
    /// 该方法与<see cref="RenderTreeBuilder"/> 的 <c>builder.AddAttribute(sequence,"ChildContent",content)</c> 方法一样。
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="content">框架的内容。是 <see cref="string"/> 或 <see cref="RenderFragment"/> 类型。</param>
    internal static RenderTreeBuilder AddChildContentAttribute(this RenderTreeBuilder builder, int sequence, object? content)
    {
        if (content is not null)
        {
            builder.AddAttribute(sequence, "ChildContent", (RenderFragment)(child =>
            {
                switch (content)
                {
                    case string or null:
                        child.AddContent(sequence, content?.ToString() ?? string.Empty);
                        break;
                    case RenderFragment fragment:
                        child.AddContent(sequence, fragment);
                        break;
                    case MarkupString markupString:
                        child.AddContent(sequence, markupString);
                        break;
                }
            }));
        }

        return builder;
    }

    /// <summary>
    /// Appends text frame to <c>Content</c>.
    /// <para>
    /// It is same as <c>builder.AddAttribute(sequence,"ChildContent",content)</c> for <see cref="RenderTreeBuilder"/> class.
    /// </para>
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="content">Content for the frame. <see cref="string"/> or <see cref="RenderFragment"/> type.</param>
    /// <returns>An attribute has added for <see cref="RenderTreeBuilder"/> instance.</returns>
    internal static RenderTreeBuilder AddChildContent(this RenderTreeBuilder builder, int sequence, object content)
    {
        if (content is not null)
        {
            switch (content)
            {
                case string:
                    builder.AddContent(sequence, content?.ToString() ?? string.Empty);
                    break;
                case RenderFragment fragment:
                    builder.AddContent(sequence, fragment);
                    break;
                case MarkupString markupString:
                    builder.AddContent(sequence, markupString);
                    break;
            }
        }

        return builder;
    }
}
