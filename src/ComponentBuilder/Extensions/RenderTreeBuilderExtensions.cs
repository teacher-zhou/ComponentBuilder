using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

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
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="elementName">HTML element name.</param>
    /// <param name="content">Tag content of the element.</param>
    /// <param name="attributes">Attributes of HTML elements.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> is null or empty string.</exception>
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
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="elementName">HTML element name.</param>
    /// <param name="content">content of the element.</param>
    /// <param name="attributes">Attributes of HTML elements.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> is null or empty string.</exception>
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
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="elementName">HTML element name.</param>
    /// <param name="content">fragment of the element.</param>
    /// <param name="attributes">Attributes of HTML elements.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
    /// <exception cref="ArgumentException"><paramref name="elementName"/> is null or empty string.</exception>
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
    /// Creates component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="componentType">Type of component.</param>
    /// <param name="content">Tag content of the component.</param>
    /// <param name="attributes">Parameters of component or HTML attributes.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> is null.</exception>
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
    /// Creates component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="componentType">Type of component.</param>
    /// <param name="content">Tag content of the component.</param>
    /// <param name="attributes">Parameters of component or HTML attributes.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> is null.</exception>
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
    /// Creates component with the specified type.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="componentType">Type of component.</param>
    /// <param name="content">Tag content of the component.</param>
    /// <param name="attributes">Parameters of component or HTML attributes.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
    /// <exception cref="ArgumentException"><paramref name="componentType"/> is null.</exception>
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
    /// Creates component with the specified type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="content">Tag content of the component.</param>
    /// <param name="attributes">Parameters of component or HTML attributes.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
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
    /// Creates component with the specified type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="content">Tag content of the component.</param>
    /// <param name="attributes">Parameters of component or HTML attributes.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
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
    /// Creates component with the specified type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create element.</param>
    /// <param name="sequence">The integer where the instruction is located in the source code.</param>
    /// <param name="content">Tag content of the component.</param>
    /// <param name="attributes">Parameters of component or HTML attributes.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <param name="condition">The conditions for creating an HTML element are met.</param>
    /// <param name="key">Specifies the key value for this element.</param>
    /// <param name="captureReference">The operation that is invoked when the reference value changes.。</param>
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
    /// Creates a cascading component of the specified value.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to create.</param>
    /// <param name="value">Value of the cascading parameter.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="content">Delegate that renders the UI content of this element.</param>
    /// <param name="name">Cascading parameter name.</param>
    /// <param name="isFixed">If it is <c>true</c>, the Value of <see cref="CascadingValue{TValue}.Value "/> cannot be modified. This is a performance optimization that allows the framework to skip Settings change notifications.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> or <paramref name="content"/> is null。
    /// </exception>
    public static void CreateCascadingComponent<TValue>(this RenderTreeBuilder builder,
                                                        TValue value,
                                                        int sequence,
                                                        [NotNull] RenderFragment? content,
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
    /// Create a region of style，such as <c>&lt;style>...&lt;/style></c>。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> instance.</param>
    /// <param name="sequence">An integer indicating the position of the instruction in the source code.</param>
    /// <param name="selector">Performs an action of selector.</param>
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
