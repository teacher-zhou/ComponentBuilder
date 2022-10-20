namespace ComponentBuilder.Render;
public class BlazorTreeRender : IDisposable
{
    private readonly RenderTreeBuilder _builder;

    internal BlazorTreeRender(RenderTreeBuilder builder,int? sequence=default)
    {
        _builder = builder;
        _builder.OpenRegion(sequence ?? new Guid().GetHashCode());
    }
    public TreeRenderType? Type { get; private set; }

    private int Sequence { get; set; }


    internal BlazorTreeRender Begin(string elementName, int sequence = 0)
    {
        Type = TreeRenderType.Element;
        Sequence = sequence;
        _builder.OpenElement(Sequence, elementName);
        return this;
    }

    internal BlazorTreeRender Begin(Type componentType, int sequence = 0)
    {
        Type = TreeRenderType.Component;
        Sequence = sequence;
        _builder.OpenComponent(Sequence, componentType);
        return this;
    }

    internal BlazorTreeRender Begin<TComponent>(int sequence = 0) where TComponent : IComponent
    {
        Type = TreeRenderType.Component;
        Sequence = sequence;
        _builder.OpenComponent<TComponent>(Sequence);
        return this;
    }

    public BlazorTreeRender Attributes(OneOf<IEnumerable<KeyValuePair<string, object>>, object> attributes)
    {
        _builder.AddMultipleAttributes(Sequence++, HtmlHelper.MergeHtmlAttributes(attributes));
        return this;
    }

    public BlazorTreeRender Class(params OneOf<string?, (bool condition, string? css)>[] classes)
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
            _builder.AddAttribute(Sequence++, "class", String.Join(" ", classList));
        }

        return this;
    }

    public BlazorTreeRender Style(params OneOf<string?, (bool condition, string? style)>[] styles)
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
            _builder.AddAttribute(Sequence++, "style", String.Join(" ", styleList));
        }

        return this;
    }

    public BlazorTreeRender EventCallback(string name, EventCallback callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
        }

        _builder.AddAttribute(Sequence++, name, callback);
        return this;
    }

    public BlazorTreeRender EventCallback<TEventArgs>(string name, EventCallback<TEventArgs> callback)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"“{nameof(name)}”不能为 null 或空。", nameof(name));
        }

        _builder.AddAttribute(Sequence++, name, callback);
        return this;
    }

    public BlazorTreeRender Content(string? content)
    {
        _builder.AddContent(Sequence++, content);
        return this;
    }

    public BlazorTreeRender Content(RenderFragment? content)
    {
        _builder.AddContent(Sequence++, content);
        return this;
    }

    public HtmlReference? End(bool captureReference = default)
    {
        HtmlReference? reference = default;
        if (captureReference)
        {
            if (Type == TreeRenderType.Element)
            {
                _builder.AddElementReferenceCapture(Sequence++, element =>
                {
                    reference = new(element);
                });
            }
            else
            {
                _builder.AddComponentReferenceCapture(Sequence++, component =>
                {
                    reference = new(component);
                });
            }
        }
        ((IDisposable)this).Dispose();

        return reference;
    }

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

public enum TreeRenderType
{
    Element,
    Component
}
