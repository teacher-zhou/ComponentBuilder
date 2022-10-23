namespace ComponentBuilder.Rendering;

/// <summary>
/// Represents a blazor render tree to build <see cref="RenderTreeBuilder"/> .
/// </summary>
public sealed class BlazorRenderTree : IDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorRenderTree"/> class.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="regionSequence">The sequence of this region. <c>null</c> to create randomly.</param>
    internal BlazorRenderTree(RenderTreeBuilder builder, int? regionSequence = default)
    {
        Builder = builder;
        Builder.OpenRegion(regionSequence ?? Guid.NewGuid().GetHashCode());
    }

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    RenderTreeType? Type { get; set; }

    /// <summary>
    /// Gets or sets the sequence of source code.
    /// </summary>
    internal int Sequence { get; set; }

    /// <summary>
    /// For inernal use only.
    /// </summary>
    internal RenderTreeBuilder Builder { get; }

    /// <summary>
    /// A boolean value representing the method <see cref="Class"/> has called.
    /// </summary>
    bool HasClassCalled { get; set; }
    /// <summary>
    /// A boolean value representing the method <see cref="Style"/> has called.
    /// </summary>
    bool HasStyleCalled { get; set; }

    #region Open
    /// <summary>
    /// Represents an open element with specified name.
    /// <param name="elementName">A value representing the type of the element.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open element.</returns>
    public BlazorRenderTree Open(string elementName, int sequence = 0)
    {
        Type = RenderTreeType.Element;
        Sequence = sequence;
        Builder.OpenElement(Sequence, elementName);
        return this;
    }

    /// <summary>
    /// Represents an open component with specify type.
    /// <para>
    /// You have to also call <see cref="BlazorRenderTree.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
    /// <code language="cs">
    /// using var render = builder.Open(typeof(MyCompenent));
    /// </code>
    /// or
    /// <code language="cs">
    /// using(var render = bulder.Open(typeof(MyCompenent)))
    /// {
    ///     //...
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="componentType">A type of component.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open component.</returns>
    public BlazorRenderTree Open(Type componentType, int sequence = 0)
    {
        Type = RenderTreeType.Component;
        Sequence = sequence;
        Builder.OpenComponent(Sequence, componentType);
        return this;
    }

    /// <summary>
    /// Represents an open component with specify type.
    /// <para>
    /// You have to also call <see cref="BlazorRenderTree.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
    /// <code language="cs">
    /// using var render = builder.Open&lt;MyComponent>();
    /// </code>
    /// OR
    /// <code language="cs">
    /// using(var render = bulder.Open&lt;MyComponent>()
    /// {
    ///     //...
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open component.</returns>
    public BlazorRenderTree Open<TComponent>(int sequence = 0) where TComponent : IComponent
    {
        Type = RenderTreeType.Component;
        Sequence = sequence;
        Builder.OpenComponent<TComponent>(Sequence);
        return this;
    }
    #endregion

    #region Attributes
    /// <summary>
    /// Add element attributes or component parameters and attributes.
    /// </summary>
    /// <param name="attributes">The HTML attributes or component parameters</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains attrbutes or parameters.</returns>
    public BlazorRenderTree Attributes(OneOf<IEnumerable<KeyValuePair<string, object>>, object> attributes)
    {
        Builder.AddMultipleAttributes(Sequence++, HtmlHelper.MergeHtmlAttributes(attributes));
        return this;
    }

    /// <summary>
    /// Add element attribute or component parameter and attribute.
    /// </summary>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains attrbutes or parameters.</returns>
    public BlazorRenderTree Attributes<TValue>(string name, TValue? value)
    {
        Builder.AddAttribute(Sequence++, name, value);
        return this;
    }
    #endregion

    #region Class
    /// <summary>
    /// Add the 'class' attribute of element or component.
    /// <para>
    /// Example:
    /// <code language="cs">
    /// builder.Open("div")
    ///     .Class("active", (isDisabeld, "is-disabled"), (Color.HasValue, "btn-primary"))
    /// .Close();
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="classes">A array of string to add 'class' attribute.
    /// <para>
    /// This value support a single string representing css class or the key/value paires represeting a condition is true to add given class string. 
    /// </para>
    /// </param>
    /// <remarks>This method only can call once.</remarks>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains 'class' attribute.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="classes"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Method is called more than once.</exception>
    public BlazorRenderTree Class(params OneOf<string?, (bool condition, string? css)>[] classes)
    {
        if (classes is null)
        {
            throw new ArgumentNullException(nameof(classes));
        }

        if (HasClassCalled)
        {
            throw new InvalidOperationException($"The {nameof(Class)} method can only call once.");
        }

        Builder.AddClassAttribute(Sequence++, classes);
        HasClassCalled = true;
        return this;
    }
    #endregion

    #region Style
    /// <summary>
    /// Add the 'style' attribute of element or component.
    /// <para>
    /// Example:
    /// <code language="cs">
    /// builder.Open("div")
    ///     .Style("height:100px", (Active, "display:block"), (Width.HasValue, $"width:{Width}px"))
    /// .Close();
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="styles">An array of string to add 'style' attribute.
    /// <para>
    /// This value support a single string representing style value or the key/value paire string represeting a condition is <c>true</c> to add given style value. 
    /// </para>
    /// </param>
    /// <remarks>This method only can call once.</remarks>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains 'style' attribute.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="styles"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Method is called more than once.</exception>
    public BlazorRenderTree Style(params OneOf<string?, (bool condition, string? style)>[] styles)
    {
        if (styles is null)
        {
            throw new ArgumentNullException(nameof(styles));
        }

        if (HasStyleCalled)
        {
            throw new InvalidOperationException($"The {nameof(Style)} method can only call once.");
        }

        Builder.AddStyleAttribute(Sequence++, styles);

        HasStyleCalled = true;

        return this;
    }
    #endregion

    #region EventCallback
    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// <para>
    /// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    /// </para>
    /// <para>
    /// Example:
    /// <code language="cs">
    /// Htmlhelper.CreateCallback(this, () => { 
    ///     // you callback code here...
    /// })
    /// </code>
    /// </para>
    /// </param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public BlazorRenderTree EventCallback(string name, EventCallback callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        return Attributes(name, callback);
    }

    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// <para>
    /// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    /// </para>
    /// <para>
    /// Example:
    /// <code language="cs">
    /// Htmlhelper.CreateCallback&lt;TEventArgs>(this, (e) => { 
    ///     // you callback code here...
    /// })
    /// </code>
    /// </para>
    /// </param>
    /// <typeparam name="TEventArgs">The argument of event.</typeparam>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public BlazorRenderTree EventCallback<TEventArgs>(string name, EventCallback<TEventArgs> callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        return Attributes(name, callback);
    }
    #endregion

    #region Content
    /// <summary>
    /// Add text string to this element.
    /// <para>
    /// <note type="tip">
    /// NOTE: This operation can only be done after the attributes or parameters has been added.
    /// </note>
    /// </para>
    /// </summary>
    /// <param name="content">The text string to insert into inner element.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains inner content.</returns>
    public BlazorRenderTree Content(string? content)
        => Content(builder => builder.AddContent(0, content));

    /// <summary>
    /// Add fragment content to this element or component.
    /// <para>
    /// <note type="tip">
    /// NOTE: This operation can only be done after the attributes or parameters has been added.
    /// </note>
    /// </para>
    /// </summary>
    /// <param name="content">The fragment of content to insert into inner element.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains inner content.</returns>
    public BlazorRenderTree Content(RenderFragment? content)
    {
        if (Type == RenderTreeType.Element)
        {
            Builder.AddContent(Sequence++, content);
        }
        else
        {
            Builder.AddAttribute(Sequence++, "ChildContent", content);
        }
        return this;
    }
    /// <summary>
    /// Add a fragment with specified value to inner component. NORMALLY, it is used to create child content for component.
    /// <para>
    /// <note type="tip">
    /// NOTE: This operation can only be done after the attributes or parameters has been added.
    /// </note>
    /// </para>
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="content">A fragment content.</param>
    /// <param name="value">The value of fragment context.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains inner content.</returns>
    public BlazorRenderTree Content<TValue>(RenderFragment<TValue> content, TValue value)
    {
        Builder.AddContent(Sequence++, content, value);
        return this;
    }

    /// <summary>
    /// Add inner markup string to this element or element.
    /// <para>
    /// <note type="tip">
    /// NOTE: This operation can only be done after the attributes or parameters has been added.
    /// </note>
    /// </para>
    /// </summary>
    /// <param name="content">The markup content to insert into inner element.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains inner content.</returns>
    public BlazorRenderTree Content(MarkupString content)
        => Content(builder => builder.AddContent(0, content));

    #endregion

    /// <summary>
    /// Assigns the specified key value to the current element or component.
    /// </summary>
    /// <param name="value">The value for the key.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance that has set key value.</returns>
    public BlazorRenderTree Key(object? value)
    {
        Builder.SetKey(value);
        return this;
    }

    /// <summary>
    /// Captures the reference for current element or component.
    /// <para>
    /// NOTE: This can only be done after the attributes and parameters have been added and before the content has been added.
    /// </para>
    /// </summary>
    /// <param name="reference">Output the referene of element or component that captured.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance that reference is captured.</returns>
    public BlazorRenderTree Reference(out HtmlReference? reference)
    {
        HtmlReference? htmlReference = default;
        if (Type == RenderTreeType.Element)
        {
            Builder.AddElementReferenceCapture(Sequence++, element =>
            {
                htmlReference = new(element);
            });
        }
        else
        {
            Builder.AddComponentReferenceCapture(Sequence++, component =>
            {
                htmlReference = new(component);
            });
        }
        reference = htmlReference;
        return this;
    }

    /// <summary>
    /// Marks a previously appended element or component as closed. Calls to this method
    /// must be balanced with calls to <c>Open()</c>.
    /// </summary>
    public void Close() => ((IDisposable)this).Dispose();

    /// <inheritdoc/>
    void IDisposable.Dispose()
    {
        switch (Type)
        {
            case RenderTreeType.Element:
                Builder.CloseElement();
                break;
            case RenderTreeType.Component:
                Builder.CloseComponent();
                break;
            default:
                break;
        }
        Builder.CloseRegion();
    }
}

/// <summary>
/// The type of render tree.
/// </summary>
internal enum RenderTreeType
{
    /// <summary>
    /// The element.
    /// </summary>
    Element,
    /// <summary>
    /// The component.
    /// </summary>
    Component
}
