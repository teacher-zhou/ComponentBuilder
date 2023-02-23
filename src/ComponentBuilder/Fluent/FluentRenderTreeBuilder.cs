namespace ComponentBuilder.Fluent;

/// <summary>
/// A fluent API to build render tree.
/// </summary>
public interface IFluentRenderTreeBuilder : IFluentOpenBuilder, IFluentAttributeBuilder,IFluentFrameBuilder, IFluentContentBuilder
{
}

/// <summary>
/// Represents a fluent API to build <see cref="RenderTreeBuilder"/> .
/// </summary>
internal sealed class FluentRenderTreeBuilder : IFluentRenderTreeBuilder
{
    RenderTreeType _treeType = RenderTreeType.None;
    private readonly RenderTreeBuilder _builder;
    private object _openInstance;
    private Dictionary<string, List<object>> _keyValuePairs = new();
    private Dictionary<string, object> _htmlAttributes = new();
    private object? _key;

    private List<RenderFragment> _contents = new();
    private Action<object>? _capture;
    private int _sequence = -1;
    private bool _hasRegion;

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentRenderTreeBuilder"/> class.
    /// </summary>
    /// <param name="builder">The builder.</param>
    internal FluentRenderTreeBuilder(RenderTreeBuilder builder)
    {
        _builder = builder;
    }

    /// <inheritdoc/>
    public IFluentOpenBuilder Region(int? sequence = default)
    {
        _builder.OpenRegion(sequence ?? Guid.NewGuid().GetHashCode());
        _hasRegion = true;
        return this;
    }

    /// <summary>
    /// Represents an open element with specified name.
    /// <param name="elementName">A value representing the type of the element.</param>
    /// </summary>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public IFluentAttributeBuilder Element(string elementName)
    {
        _treeType = RenderTreeType.Element;
        _openInstance = elementName;
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
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public IFluentAttributeBuilder Component(Type componentType)
    {
        _treeType = RenderTreeType.Component;
        _openInstance = componentType;
        return this;
    }

    #region Attributes

    /// <summary>
    /// Add element attribute or component parameter and attribute.
    /// </summary>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains attrbutes or parameters.</returns>
    public IFluentAttributeBuilder Attribute(string name, object? value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }

        if (value is not null)
        {
            if (_keyValuePairs.TryGetValue(name, out var result))
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

    #region Content

    /// <summary>
    /// Add fragment content to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="fragment">The fragment of content to insert into inner element.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    public IFluentContentBuilder Content(RenderFragment? fragment)
    {
        if (fragment is not null)
        {
            _contents.Add(fragment);
        }

        return this;
    }
    #endregion

    /// <summary>
    /// Assigns the specified key value to the current element or component.
    /// </summary>
    /// <param name="value">The value for the key.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance that has set key value.</returns>
    public IFluentAttributeBuilder Key(object? value)
    {
        _key = value;
        return this;
    }

    /// <summary>
    /// Captures the reference for element.
    /// </summary>
    /// <param name="captureReferenceAction">An action to capture the reference of element after component is rendered.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance that reference is captured.</returns>
    public IFluentAttributeBuilder Ref(Action<object?> captureReferenceAction)
    {
        _capture = captureReferenceAction;
        return this;
    }

    /// <summary>
    /// Marks a previously appended element or component as closed. Calls to this method
    /// must be balanced with calls to <c>Element()</c> or <c>Component</c>.
    /// </summary>
    public IFluentOpenBuilder Close()
    {
        ((IDisposable)this).Dispose();
        return this;
    }

    /// <inheritdoc/>
    void IDisposable.Dispose()
    {
        if ( _treeType != RenderTreeType.None )
        {
            Build();
        }

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
        if ( _hasRegion )
        {
            _builder.CloseRegion();
            _hasRegion = false;
        }
        _treeType = RenderTreeType.None;
        _contents.Clear();
        _sequence = -1;
    }

    /// <summary>
    /// Build the render tree.
    /// </summary>
    void Build()
    {
        BuildOpen();
        BuildKey();
        BuildAttributes();
        CaptureReference();
        BuildContents();

        void BuildOpen()
        {
            _sequence = Guid.NewGuid().GetHashCode();

            switch ( _treeType )
            {
                case RenderTreeType.Element:
                    _builder.OpenElement(_sequence, _openInstance.ToString());
                    break;
                case RenderTreeType.Component:
                    _builder.OpenComponent(_sequence, (Type)_openInstance);
                    break;
                default:
                    break;
            }
        }

        void BuildContents()
        {
            foreach (var content in _contents)
            {
                _builder.AddContent(_sequence++, content);
            }
        }

        void CaptureReference()
        {
            if (_capture is not null)
            {
                if (_treeType == RenderTreeType.Component)
                {
                    _builder.AddComponentReferenceCapture(_sequence++, _capture);
                }
                else
                {
                    _builder.AddElementReferenceCapture(_sequence++, e => _capture(e));
                }
            }
        }

        void BuildAttributes()
        {
            foreach (var item in _keyValuePairs)
            {
                var name = item.Key;

                foreach (var value in item.Value)
                {
                    if (_htmlAttributes.TryGetValue(name, out var htmlValue))
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

            _builder.AddMultipleAttributes(_sequence++, _htmlAttributes);
        }

        void BuildKey()
        {
            if (_key is not null)
            {
                _builder.SetKey(_key);
            }
        }
    }
}

/// <summary>
/// The type of render tree.
/// </summary>
internal enum RenderTreeType
{
    /// <summary>
    /// Nothing
    /// </summary>
    None,
    /// <summary>
    /// The element.
    /// </summary>
    Element,
    /// <summary>
    /// The component.
    /// </summary>
    Component,
}
