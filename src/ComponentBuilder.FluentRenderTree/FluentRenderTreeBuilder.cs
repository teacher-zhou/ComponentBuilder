using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.FluentRenderTree;

public interface IFluentRenderTreeBuilder<TComponent> : IFluentRenderTreeBuilder, IFluentOpenComponentBuilder<TComponent>, IFluentAttributeBuilder<TComponent>,IFluentContentBuilder<TComponent>, IFluentCloseBuilder<TComponent>
    where TComponent : IComponent
{ }

internal class FluentRenderTreeBuilder<TComponent> : FluentRenderTreeBuilder, IFluentRenderTreeBuilder<TComponent>
    where TComponent : IComponent
{
    internal FluentRenderTreeBuilder(RenderTreeBuilder builder) : base(builder)
    {
    }

    public IFluentAttributeBuilder<TComponent> Attribute<TValue>(Expression<Func<TComponent,TValue>> parameter, TValue? value)
    {
        ArgumentNullException.ThrowIfNull(parameter);

        var name = (parameter.Body as MemberExpression)!.Member.Name;
        Attribute(name, value);
        return this;
    }

    public IFluentAttributeBuilder<TComponent> Component(int? sequence = null)
    {
        Component(typeof(TComponent), sequence);
        return this;
    }

    IFluentOpenComponentBuilder<TComponent> IFluentCloseBuilder<TComponent>.Close()
    {
        Close();
        return this;
    }

    IFluentAttributeBuilder<TComponent> IFluentContentBuilder<TComponent>.Content(RenderFragment? fragment)
    {
        Attribute("ChildContent", fragment);
        return this;
    }
}

public interface IFluentRenderTreeBuilder : IFluentOpenBuilder, IFluentAttributeBuilder, IFluentFrameBuilder, IFluentContentBuilder
{
}


internal class FluentRenderTreeBuilder : IFluentRenderTreeBuilder
{
    RenderTreeType _treeType = RenderTreeType.None;
    private object _openInstance;
    private Dictionary<string, List<object>> _keyValuePairs = [];
    private Dictionary<string, object> _htmlAttributes = [];
    private object? _key;
    private IComponentRenderMode? _renderMode;

    private List<RenderFragment> _contents = [];
    private Action<object>? _capture;
    private int _sequence = -1;
    private bool _isClosed = true;

    internal RenderTreeBuilder Builder { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="FluentRenderTreeBuilder"/> class.
    /// </summary>
    /// <param name="builder">The builder.</param>
    internal FluentRenderTreeBuilder(RenderTreeBuilder builder)
    {
        Builder = builder;
    }

    /// <inheritdoc/>
    public IFluentOpenBuilder Region(int sequence)
    {
        CheckFlushed();

        _treeType = RenderTreeType.Region;
        _sequence = GetSequence(sequence);
        return this;
    }

    /// <inheritdoc/>
    public IFluentAttributeBuilder Element(string name, int? sequence = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"“{nameof(name)}”不能为 null 或空白。", nameof(name));
        }

        CheckFlushed();
        
        _treeType = RenderTreeType.Element;
        _openInstance = name;
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
                _keyValuePairs.Add(name, [value]);
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
        Builder.AddContent(GetSequence(sequence), fragment);
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

    public IFluentAttributeBuilder RenderMode(IComponentRenderMode mode)
    {
        _renderMode = mode;
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
                    Builder.OpenElement(_sequence, _openInstance!.ToString()!);
                    break;
                case RenderTreeType.Component:
                    Builder.OpenComponent(_sequence, (Type)_openInstance);
                    break;
                case RenderTreeType.Region:
                    Builder.OpenRegion(_sequence);
                    break;
                default:
                    break;
            }
        }

        void BuildContents()
        {
            foreach (var content in _contents)
            {
                Builder.AddContent(_sequence++, content);
            }
        }

        void CaptureReference()
        {
            if ( _treeType == RenderTreeType.Component )
            {
                Builder.AddComponentReferenceCapture(_sequence++, obj => _capture?.Invoke(obj));
            }
            else
            {
                Builder.AddElementReferenceCapture(_sequence++, element =>
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
                        var result = htmlValue switch
                        {
                            string => string.Concat(htmlValue, value),
                            _ => value,
                        };

                        _htmlAttributes[name] = result;
                    }
                    else
                    {
                        //var trimedValue = Trim(value);
                        _htmlAttributes.Add(name, value);
                    }
                }
            }

            HandleFinalValue();

            Builder.AddMultipleAttributes(_sequence++, _htmlAttributes);

            object Trim(object value) => value switch
            {
                string => value!.ToString()!.Trim(),
                _ => value
            };

            void HandleFinalValue()
            {
                if ( _htmlAttributes.TryGetValue("class", out var @class) )
                {
                    _htmlAttributes["class"] = Trim(@class);
                }
            }
        }

        void BuildKey()
        {
            if (_key is not null)
            {
                Builder.SetKey(_key);
            }
        }

        void BuildRenderMode()
        {
            if(_renderMode is not null)
            {
                Builder.AddComponentRenderMode(_renderMode);
            }
        }

        void BuildClose()
        {
            switch ( _treeType )
            {
                case RenderTreeType.Element:
                    Builder.CloseElement();
                    break;
                case RenderTreeType.Component:
                    Builder.CloseComponent();
                    break;
                case RenderTreeType.Region:
                    Builder.CloseRegion();
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
    static int GetSequence(int? sequence = default) => sequence ?? new Random().Next(1000, 9999);
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
