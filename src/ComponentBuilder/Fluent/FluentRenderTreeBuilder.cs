namespace ComponentBuilder.Fluent;

/// <summary>
/// Represents a blazor render tree to build <see cref="RenderTreeBuilder"/> .
/// </summary>
public sealed class FluentRenderTreeBuilder : IDisposable
{
    RenderTreeType? _treeType = default;
    private readonly RenderTreeBuilder _builder;
    private Dictionary<string, List<object>> _keyValuePairs = new();

    private Dictionary<string, object> _htmlAttributes = new();
    private object? _key;

    private List<RenderFragment> _contents = new();
    private Action<object>? _capture;
    private int _sequence;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentRenderTreeBuilder"/> class.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <param name="regionSequence">The sequence of this region. <c>null</c> to create randomly.</param>
    internal FluentRenderTreeBuilder(RenderTreeBuilder builder, int? regionSequence = default)
    {
        _builder = builder;
        _builder.OpenRegion(regionSequence ?? Guid.NewGuid().GetHashCode());
    }


    #region Open
    /// <summary>
    /// Represents an open element with specified name.
    /// <param name="elementName">A value representing the type of the element.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.</param>
    /// </summary>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public FluentRenderTreeBuilder Element(string elementName, int sequence = 0)
    {
        _treeType = RenderTreeType.Element;
        _sequence = sequence;
        _builder.OpenElement(_sequence, elementName);
        return this;
    }

    /// <summary>
    /// Represents an open component with specify type.
    /// <para>
    /// You have to also call <see cref="FluentRenderTreeBuilder.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
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
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public FluentRenderTreeBuilder Component(Type componentType, int sequence = 0)
    {
        _treeType = RenderTreeType.Component;
        _sequence = sequence;
        _builder.OpenComponent(_sequence, componentType);
        return this;
    }

    /// <summary>
    /// Represents an open component with specify type.
    /// <para>
    /// You have to also call <see cref="FluentRenderTreeBuilder.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
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
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public FluentRenderTreeBuilder Component<TComponent>(int sequence = 0) where TComponent : IComponent
    {
        _treeType = RenderTreeType.Component;
        _sequence = sequence;
        _builder.OpenComponent<TComponent>(_sequence);
        return this;
    }
    #endregion

    #region Attributes
    ///// <summary>
    ///// Add element attributes or component parameters and attributes.
    ///// </summary>
    ///// <param name="attributes">
    ///// The HTML attributes or component parameters.
    ///// It supports a collection ot key/value pair represents attribute name and value, or anonymouse object like:
    ///// <c>
    ///// new { name = "name", title = "my title", id = "my-id", ... }
    ///// </c>
    ///// </param>
    ///// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains attrbutes or parameters.</returns>
    //public FluentRenderTreeBuilder Attributes(IEnumerable<KeyValuePair<string, object>>? attributes)
    //{
    //    if(attributes == null )
    //    {
    //        ArgumentNullException.ThrowIfNull(attributes, nameof(attributes));
    //    }

    //    foreach ( var item in attributes )
    //    {
    //        _htmlAttributes.Add(item.Key, item.Value);
    //    }
    //    return this;
    //}

    /// <summary>
    /// Add element attribute or component parameter and attribute.
    /// </summary>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains attrbutes or parameters.</returns>
    public FluentRenderTreeBuilder Attribute(string name, object? value)
    {
        if ( string.IsNullOrWhiteSpace(name) )
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        if ( value is not null )
        {
            if ( _keyValuePairs.TryGetValue(name, out var result) )
            {
                result.Add(value);
            }
            else
            {
                _keyValuePairs.Add(name, new() { value });
            }
        }
        return this;
    }
    #endregion

    //#region Class
    ///// <summary>
    ///// Add the 'class' attribute of element or component.
    ///// <para>
    ///// Example:
    ///// <code language="cs">
    ///// builder.Open("div")
    /////     .Class("active", (isDisabeld, "is-disabled"), (Color.HasValue, "btn-primary"))
    ///// .Close();
    ///// </code>
    ///// </para>
    ///// </summary>
    ///// <param name="classes">A array of string to add 'class' attribute.
    ///// <para>
    ///// This value support a single string representing css class or the key/value paires represeting a condition is true to add given class string. 
    ///// </para>
    ///// </param>
    ///// <remarks>This method only can call once.</remarks>
    ///// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains 'class' attribute.</returns>
    ///// <exception cref="ArgumentNullException"><paramref name="classes"/> is null.</exception>
    ///// <exception cref="InvalidOperationException">Method is called more than once.</exception>
    //public FluentRenderTreeBuilder Class(string? cssClass)
    //{
    //    if ( cssClass is null)
    //    {
    //        ArgumentNullException.ThrowIfNull(cssClass, nameof(cssClass));
    //    }

    //    Set("class",$"{cssClass} ");

    //    return this;
    //}
    //#endregion

    //#region Style
    ///// <summary>
    ///// Add the 'style' attribute of element or component.
    ///// <para>
    ///// Example:
    ///// <code language="cs">
    ///// builder.Open("div")
    /////     .Style("height:100px", (Active, "display:block"), (Width.HasValue, $"width:{Width}px"))
    ///// .Close();
    ///// </code>
    ///// </para>
    ///// </summary>
    ///// <param name="styles">An array of string to add 'style' attribute.
    ///// <para>
    ///// This value support a single string representing style value or the key/value paire string represeting a condition is <c>true</c> to add given style value. 
    ///// </para>
    ///// </param>
    ///// <remarks>This method only can call once.</remarks>
    ///// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains 'style' attribute.</returns>
    ///// <exception cref="ArgumentNullException"><paramref name="styles"/> is null.</exception>
    ///// <exception cref="InvalidOperationException">Method is called more than once.</exception>
    //public FluentRenderTreeBuilder Style(string? style)
    //{
    //    if ( style is null )
    //    {
    //        ArgumentNullException.ThrowIfNull(style, nameof(style));
    //    }

    //    Set("style",$"{style};");
    //    return this;
    //}
    //#endregion

    //#region EventCallback
    ///// <summary>
    ///// Add callback delegate to specify name of attribute or component.
    ///// </summary>
    ///// <param name="name">The element event name.</param>
    ///// <param name="callback">
    ///// A delegate supplies to this event. 
    ///// <para>
    ///// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    ///// </para>
    ///// <para>
    ///// Example:
    ///// <code language="cs">
    ///// Htmlhelper.Event.Create(this, () => { 
    /////     // you callback code here...
    ///// })
    ///// </code>
    ///// </para>
    ///// </param>
    ///// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    ///// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    //public FluentRenderTreeBuilder EventCallback(string name, EventCallback callback)
    //{
    //    if (string.IsNullOrEmpty(name))
    //    {
    //        throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
    //    }

    //    return Attribute(name, callback);
    //}

    ///// <summary>
    ///// Add callback delegate to specify name of attribute or component.
    ///// </summary>
    ///// <param name="name">The element event name.</param>
    ///// <param name="callback">
    ///// A delegate supplies to this event. 
    ///// <para>
    ///// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    ///// </para>
    ///// <para>
    ///// Example:
    ///// <code language="cs">
    ///// Htmlhelper.CreateCallback&lt;TEventArgs>(this, (e) => { 
    /////     // you callback code here...
    ///// })
    ///// </code>
    ///// </para>
    ///// </param>
    ///// <typeparam name="TEventArgs">The argument of event.</typeparam>
    ///// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    ///// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    //public FluentRenderTreeBuilder EventCallback<TEventArgs>(string name, EventCallback<TEventArgs> callback)
    //{
    //    if (string.IsNullOrEmpty(name))
    //    {
    //        throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
    //    }

    //    return Attribute(name, callback);
    //}
    //#endregion

    #region Content
    ///// <summary>
    ///// Add text string to this element or component. Multiple content will be combined for multiple invocation.
    ///// <para>
    ///// <note type="tip">
    ///// NOTE: This operation can only be done after the attributes or parameters has been added.
    ///// </note>
    ///// </para>
    ///// </summary>
    ///// <param name="content">The text string to insert into inner element.</param>
    ///// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    //public FluentRenderTreeBuilder Content(string? content)
    //    => Content(builder => builder.AddContent(0, content));

    /// <summary>
    /// Add fragment content to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="content">The fragment of content to insert into inner element.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    public FluentRenderTreeBuilder Content(RenderFragment content)
    {
        if ( content is not null )
        {
            _contents.Add(content);
        }

        return this;
    }
    /// <summary>
    /// Add a fragment with specified value to inner component. 
    /// Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="content">A fragment content.</param>
    /// <param name="value">The value of fragment context.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    public FluentRenderTreeBuilder Content<TValue>(RenderFragment<TValue> content, TValue value) => Content(content(value));

    

    #endregion

    /// <summary>
    /// Assigns the specified key value to the current element or component.
    /// </summary>
    /// <param name="value">The value for the key.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance that has set key value.</returns>
    public FluentRenderTreeBuilder Key(object? value)
    {
        _key = value;
        return this;
    }

    /// <summary>
    /// Captures the reference for element.
    /// <para>
    /// NOTE: This can only be done after the attributes or parameters have been added and before the content has been added.
    /// </para>
    /// </summary>
    /// <param name="captureReferenceAction">An action to capture the reference of element after component is rendered.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance that reference is captured.</returns>
    public FluentRenderTreeBuilder Ref(Action<ElementReference> captureReferenceAction)
    {
        _capture?.Invoke(captureReferenceAction);
        return this;
    }

    /// <summary>
    /// Captures the reference for component.
    /// <para>
    /// NOTE: This can only be done after the attributes or parameters have been added and before the content has been added.
    /// </para>
    /// </summary>
    /// <param name="captureReferenceAction">An action to capture the reference of component after component is rendered.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance that reference is captured.</returns>
    public FluentRenderTreeBuilder Ref<TComponent>(Action<TComponent> captureReferenceAction) where TComponent : IComponent
    {
        _capture?.Invoke(captureReferenceAction);

        return this;
    }

    /// <summary>
    /// Marks a previously appended element or component as closed. Calls to this method
    /// must be balanced with calls to <c>Element()</c> or <c>Component</c>.
    /// </summary>
    public FluentRenderTreeBuilder Close()
    {
        ((IDisposable)this).Dispose();
        return this;
    }

    /// <inheritdoc/>
    void IDisposable.Dispose()
    {
        ThrowIfTreeTypeNull();
        Build();
        switch ( _treeType )
        {
            case RenderTreeType.Element:
                _builder.CloseElement();
                break;
            case RenderTreeType.Component:
                _builder.CloseComponent();
                break;
            default:
                break;
        }
        _builder.CloseRegion();
        _treeType = default;
    }

    void Build()
    {
        foreach ( var item in _keyValuePairs )
        {
            var name = item.Key;

            foreach ( var value in item.Value )
            {
                if ( _htmlAttributes.TryGetValue(name, out var htmlValue) )
                {
                    _htmlAttributes[name] = htmlValue switch
                    {
                        string => string.Concat(htmlValue, value),
                        _ => value,
                    };
                }
                else
                {
                    _htmlAttributes.Add(name, value);
                }
            }

        }

        if ( _key is not null )
        {
            _builder.SetKey(_key);
        }

        if ( _capture is not null )
        {
            if ( _treeType == RenderTreeType.Component )
            {
                _builder.AddComponentReferenceCapture(_sequence++, _capture);
            }
            else
            {
                _builder.AddElementReferenceCapture(_sequence++, e => _capture(e));
            }
        }

        _builder.AddMultipleAttributes(_sequence++, _htmlAttributes);

        foreach ( var content in _contents )
        {
            _builder.AddContent(_sequence++, content);
        }
    }

    private void ThrowIfTreeTypeNull()
    {
        if ( _treeType is null )
        {
            throw new InvalidOperationException("Use Open to start component or element first");
        }
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
