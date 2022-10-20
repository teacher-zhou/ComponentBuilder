namespace ComponentBuilder.Rendering;

/// <summary>
/// Represents a blazor render tree.
/// </summary>
public sealed class BlazorRenderTree : IDisposable
{
    private readonly RenderTreeBuilder _builder;

    internal BlazorRenderTree(RenderTreeBuilder builder, int? sequence = default)
    {
        _builder = builder;
        _builder.OpenRegion(sequence ?? new Guid().GetHashCode());
    }
    TreeRenderType? Type { get; set; }

    private int Sequence { get; set; }


    public BlazorRenderTree Open(string elementName, int sequence = 0)
    {
        Type = TreeRenderType.Element;
        Sequence = sequence;
        _builder.OpenElement(Sequence, elementName);
        return this;
    }

    public BlazorRenderTree Open(Type componentType, int sequence = 0)
    {
        Type = TreeRenderType.Component;
        Sequence = sequence;
        _builder.OpenComponent(Sequence, componentType);
        return this;
    }

    public BlazorRenderTree Open<TComponent>(int sequence = 0) where TComponent : IComponent
    {
        Type = TreeRenderType.Component;
        Sequence = sequence;
        _builder.OpenComponent<TComponent>(Sequence);
        return this;
    }

    #region Attributes
    public BlazorRenderTree Attributes(OneOf<IEnumerable<KeyValuePair<string, object>>, object> attributes)
    {
        _builder.AddMultipleAttributes(Sequence++, HtmlHelper.MergeHtmlAttributes(attributes));
        return this;
    }
    public BlazorRenderTree Attributes(string name, object? value)
    {
        _builder.AddAttribute(Sequence++, name, value);
        return this;
    }
    #endregion

    public BlazorRenderTree Class(params OneOf<string?, (bool condition, string? css)>[] classes)
    {
        if (classes is null)
        {
            throw new ArgumentNullException(nameof(classes));
        }

        var classList = new List<string>();
        foreach (var item in classes)
        {
            item.Switch(value => Class((true, value)),
                value =>
                {
                    if (value.condition && !string.IsNullOrEmpty(value.css))
                    {
                        classList.Add(value.css);
                    }
                }
            );
        }

        if (classList.Any())
        {
            _builder.AddAttribute(Sequence++, "class", string.Join(" ", classList));
        }

        return this;
    }

    public BlazorRenderTree Style(params OneOf<string?, (bool condition, string? style)>[] styles)
    {
        if (styles is null)
        {
            throw new ArgumentNullException(nameof(styles));
        }

        var styleList = new List<string>();
        foreach (var item in styles)
        {
            item.Switch(value => Style((true, value)), value =>
            {
                if (value.condition && !string.IsNullOrEmpty(value.style))
                {
                    styleList.Add(value.style);
                }
            });
        }

        if (styleList.Any())
        {
            _builder.AddAttribute(Sequence++, "style", string.Join(" ", styleList));
        }

        return this;
    }
    #region EventCallback
    public BlazorRenderTree EventCallback(string name, EventCallback callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
        }

        _builder.AddAttribute(Sequence++, name, callback);
        return this;
    }

    public BlazorRenderTree EventCallback<TEventArgs>(string name, EventCallback<TEventArgs> callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
        }

        _builder.AddAttribute(Sequence++, name, callback);
        return this;
    }
    #endregion

    #region Content
    public BlazorRenderTree Content(string? content)
        => Content(builder => builder.AddContent(0, content));

    public BlazorRenderTree Content(RenderFragment? content)
    {
        _builder.AddContent(Sequence++, content);
        return this;
    }

    public BlazorRenderTree Content(MarkupString content)
        => Content(builder => builder.AddContent(0, content));

    public BlazorRenderTree Content<TValue>(RenderFragment<TValue> content, TValue value)
    {
        _builder.AddContent(Sequence++, content, value);
        return this;
    }
    #endregion

    #region ChildContent
    public BlazorRenderTree ChildContent(string? content)
        => ChildContent(builder => builder.AddContent(0, content));

    public BlazorRenderTree ChildContent(RenderFragment? content)
    {
        _builder.AddAttribute(Sequence++, nameof(ChildContent), content);
        return this;
    }

    public BlazorRenderTree ChildContent(MarkupString content)
        => ChildContent(builder => builder.AddContent(0, content));
    #endregion

    public BlazorRenderTree Key(object? value)
    {
        _builder.SetKey(value);
        return this;
    }

    public BlazorRenderTree Reference(out HtmlReference? reference)
    {
        HtmlReference? htmlReference = default;
        if (Type == TreeRenderType.Element)
        {
            _builder.AddElementReferenceCapture(Sequence++, element =>
            {
                htmlReference = new(element);
            });
        }
        else
        {
            _builder.AddComponentReferenceCapture(Sequence++, component =>
            {
                htmlReference = new(component);
            });
        }
        reference = htmlReference;
        return this;
    }

    public void Close() => ((IDisposable)this).Dispose();

    void IDisposable.Dispose()
    {
        switch (Type)
        {
            case TreeRenderType.Element:
                _builder.CloseElement();
                break;
            case TreeRenderType.Component:
                _builder.CloseComponent();
                break;
            default:
                break;
        }
        _builder.CloseRegion();
    }
}

internal enum TreeRenderType
{
    Element,
    Component
}
