﻿using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder.Enhancement;

/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> class.
/// </summary>
public static class RenderTreeBuilderExtensions
{
    #region CreateElement
    /// <summary>
    /// Creates an HTML element with the specified element name.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="elementName">HTML element name.</param>
    /// <param name="content">The markup content of element.</param>
    /// <param name="attributes">The HTML attribute of the element.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create element value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this element.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> null or empty.</exception>
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
    /// Creates an HTML element with the specified element name.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="elementName">HTML element name.</param>
    /// <param name="content">The content of element.</param>
    /// <param name="attributes">The HTML attribute of the element.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create element value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this element.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> null or empty.</exception>
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
    /// Creates an HTML element with the specified element name.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="elementName">HTML element name.</param>
    /// <param name="content">The UI fragment of the element.</param>
    /// <param name="attributes">The HTML attribute of the element.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create element value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this element.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> null or empty.</exception>
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
            builder.AddMultipleAttributes(nextSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes));
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
    /// Creates a component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="componentType">Component type to create.</param>
    /// <param name="content">The markup string of component.</param>
    /// <param name="attributes">The HTML attributes or parameters of the component.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create component value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this element.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> null.</exception>
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
    /// Creates a component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="componentType">Component type to create.</param>
    /// <param name="content">The text of the component.</param>
    /// <param name="attributes">The HTML attributes or parameters of the component.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create component value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this component.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> null.</exception>
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
    /// Creates a component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="componentType">Component type to create.</param>
    /// <param name="content">The UI fragment of the component.</param>
    /// <param name="attributes">The HTML attributes or parameters of the component.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create component value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this component.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> null.</exception>
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
            builder.AddMultipleAttributes(nextSequence + 1, HtmlHelper.MergeHtmlAttributes(attributes));
        }

        builder.AddAttribute(nextSequence + 2, "ChildContent", content);

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
    /// Creates a component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>    
    /// <param name="content">The markup string of the component.</param>
    /// <param name="attributes">The HTML attributes or parameters of the component.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create component value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this component.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <typeparam name="TComponent">The type of component to create.</typeparam>
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
    /// Creates a component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>    
    /// <param name="content">The text content of the component.</param>
    /// <param name="attributes">The HTML attributes or parameters of the component.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create component value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this component.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <typeparam name="TComponent">The type of component to create.</typeparam>
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
    /// Creates a component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>    
    /// <param name="content">The UI fragment of the component.</param>
    /// <param name="attributes">The HTML attributes or parameters of the component.
    /// You can use anonymous classes, <code>new { @class="class1", id="my-id" , onclick = xxx, data_target="xxx" }</code>
    /// </param>
    /// <param name="condition">The condition to create component value is <c>true</c>.</param>
    /// <param name="key">The value of key to assign this component.</param>
    /// <param name="captureReference">An action to be invoked whenever the reference value changes.</param>
    /// <typeparam name="TComponent">The type of component to create.</typeparam>
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
    /// Create cascading component with specified value.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="value">Value of the cascade parameter.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="content">The delegate that renders the UI content of this element.</param>
    /// <param name="name">Name of the cascading parameter.</param>
    /// <param name="isFixed">if <c>true</c>, the <see cref="CascadingValue{TValue}.Value"/> is not be changed. This is a performance optimization that allows the framework to skip setting change notifications.</param>
    /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> or <paramref name="content"/> is null.
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

    /// <summary>
    /// Create a component with cascading parameters.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create component.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="content">The delegate that renders the UI content of this element.</param>
    /// <param name="name">Name of the cascading parameter.</param>
    /// <param name="isFixed">if <c>true</c>, the <see cref="CascadingValue{TValue}.Value"/> is not be changed. This is a performance optimization that allows the framework to skip setting change notifications.</param>
    /// <returns>A cascading component has created for <see cref="RenderTreeBuilder"/> instance.</returns>    
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> or <paramref name="content"/> is null.
    /// </exception>
    public static void CreateCascadingComponent([NotNull] this ComponentBase component,
                                                RenderTreeBuilder builder,
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

        builder.CreateCascadingComponent(component, sequence, content, name, isFixed);
    }
    #endregion

    #region Style

    /// <summary>
    /// Create a region of style like <c>&lt;style>...&lt;/style></c>.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="selector">An action to create style.</param>
    /// <param name="type">The style type.</param>
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