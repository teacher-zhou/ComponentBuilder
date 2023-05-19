using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// A fluent API to build render tree.
/// </summary>
public interface IFluentRenderTreeBuilder : IFluentOpenBuilder, IFluentAttributeBuilder, IFluentFrameBuilder, IFluentContentBuilder
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
    private bool _isClosed = true;

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
        CheckFlushed();

        _treeType = RenderTreeType.Region;
        _sequence = GetSequence(sequence);
        return this;
    }

    /// <inheritdoc/>
    public IFluentAttributeBuilder Element(string elementName, int? sequence = default)
    {
        CheckFlushed();
        
        _treeType = RenderTreeType.Element;
        _openInstance = elementName;
        _sequence = GetSequence(sequence);
        return this;
    }

    /// <inheritdoc/>
    public IFluentAttributeBuilder Component(Type componentType, int? sequence = default)
    {
        CheckFlushed();

        _treeType = RenderTreeType.Component;
        _openInstance = componentType;
        _sequence = GetSequence(sequence);
        return this;
    }

    /// <inheritdoc/>
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

    #region Content

    /// <inheritdoc/>
    public IFluentContentBuilder Content(RenderFragment? fragment)
    {
        if (fragment is not null)
        {
            _contents.Add(fragment);
        }

        return this;
    }

    /// <inheritdoc/>
    IFluentOpenBuilder IFluentOpenBuilder.Content(RenderFragment? fragment, int? sequence = default)
    {
        _builder.AddContent(GetSequence(sequence), fragment);
        return this;
    }
    #endregion

    /// <inheritdoc/>
    public IFluentAttributeBuilder Key(object? value)
    {
        _key = value;
        return this;
    }


    public IFluentAttributeBuilder Ref(Action<object?> captureReferenceAction)
    {
        _capture = captureReferenceAction;
        return this;
    }

    /// <inheritdoc/>
    public IFluentOpenBuilder Close()
    {
        ((IDisposable)this).Dispose();
        _isClosed = true;
        return this;
    }

    /// <inheritdoc/>
    void IDisposable.Dispose()
    {
        Flush();
        _isClosed = true;
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
        BuildClose();

        void BuildOpen()
        {
            //_sequence = Guid.NewGuid().GetHashCode();

            switch (_treeType)
            {
                case RenderTreeType.Element:
                    _builder.OpenElement(_sequence, _openInstance.ToString());
                    break;
                case RenderTreeType.Component:
                    _builder.OpenComponent(_sequence, (Type)_openInstance);
                    break;
                case RenderTreeType.Region:
                    _builder.OpenRegion(_sequence);
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
            if ( _treeType == RenderTreeType.Component )
            {
                _builder.AddComponentReferenceCapture(_sequence++, obj => _capture?.Invoke(obj));
            }
            else
            {
                _builder.AddElementReferenceCapture(_sequence++, element =>
                {
                    _capture?.Invoke(element);
                    _capture = default;
                });
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
                        var trimedValue = Trim(value);
                        _htmlAttributes.Add(name, trimedValue);
                    }
                }

            }

            _builder.AddMultipleAttributes(_sequence++, _htmlAttributes);

            object Trim(object value) => value switch
            {
                string => value!.ToString()!.Trim(),
                _ => value
            };
        }

        void BuildKey()
        {
            if (_key is not null)
            {
                _builder.SetKey(_key);
            }
        }

        void BuildClose()
        {
            switch ( _treeType )
            {
                case RenderTreeType.Element:
                    _builder.CloseElement();
                    break;
                case RenderTreeType.Component:
                    _builder.CloseComponent();
                    break;
                case RenderTreeType.Region:
                    _builder.CloseRegion();
                    break;
                default:
                    break;
            }
        }
    }

    void Flush()
    {
        if ( _treeType != RenderTreeType.None )
        {
            Build();
        }
        Reset();
    }

    void Reset()
    {
        _treeType = RenderTreeType.None;
        _contents.Clear();
        _keyValuePairs.Clear();
        _htmlAttributes.Clear();
        //_capture = default;
        //_sequence = -1;
    }

    void CheckFlushed()
    {
        if (! _isClosed )
        {
            Flush();
            _isClosed = true;
        }
        _isClosed = false;
    }

    /// <summary>
    /// Get the sequence of source code.
    /// </summary>
    /// <param name="sequence">A sequence representing source code, <c>null</c> to generate randomly.</param>
    /// <returns></returns>
    static int GetSequence(int? sequence) => sequence ?? new Random().Next(1000, 9999);
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
    /// <summary>
    /// The region.
    /// </summary>
    Region,
}
