using System.Xml.Linq;

namespace ComponentBuilder.Render;
public class BlazorTreeRender : IDisposable
{
    private readonly RenderTreeBuilder _builder;

    internal BlazorTreeRender(RenderTreeBuilder builder)
    {
        _builder = builder;
        _builder.OpenRegion(RegionSequence);
    }
    public TreeRenderType? Type { get; private set; }

    public int RegionSequence { get; private set; } = new Guid().GetHashCode();

    private int StartSequence { get; set; }

    internal BlazorTreeRender Begin(string elementName,int sequence=0)
    {
        Type = TreeRenderType.Element;
        StartSequence = sequence;
        _builder.OpenElement(StartSequence, elementName);
        return this;
    }

    internal BlazorTreeRender Begin(Type componentType,int sequence=0)
    {
        Type = TreeRenderType.Component;
        StartSequence = sequence;
        _builder.OpenComponent(StartSequence, componentType);
        return this;
    }

    internal BlazorTreeRender Begin<TComponent>(int sequence = 0) where TComponent:IComponent
    {
        Type = TreeRenderType.Component;
        StartSequence = sequence;
        _builder.OpenComponent<TComponent>(StartSequence);
        return this;
    }

    public BlazorTreeRender Attributes(OneOf<IEnumerable<KeyValuePair<string, object>>, object> attributes)
    {
        _builder.AddMultipleAttributes(StartSequence++, HtmlHelper.MergeHtmlAttributes(attributes));
        return this;
    }

    public BlazorTreeRender Class(string? cssClass)
    {
        _builder.AddClassAttribute(StartSequence++, cssClass);
        return this;
    }

    public BlazorTreeRender Class(params OneOf<string?, (bool condition, string? css)>[] classes)
    {
        if ( classes is null )
        {
            throw new ArgumentNullException(nameof(classes));
        }

        foreach (var item in classes )
        {
            item.Switch(value => Class(value), value =>
            {
                if ( value.condition )
                {
                    Class(value.css);
                }
            });
        }

        return this;
    }

    public BlazorTreeRender Content(string? content)
    {
        _builder.AddContent(StartSequence++, content);
        return this;
    }

    public BlazorTreeRender Content(RenderFragment? content)
    {
        _builder.AddContent(StartSequence, content);
        return this;
    }
    
    public HtmlReference? End(bool captureReference=default)
    {
        HtmlReference? reference = default;
        if ( captureReference )
        {
            if ( Type == TreeRenderType.Element )
            {
                _builder.AddElementReferenceCapture(StartSequence++, element =>
                {
                    reference = new(element);
                });
            }
            else
            {
                _builder.AddComponentReferenceCapture(StartSequence++, component =>
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
        switch ( Type )
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
