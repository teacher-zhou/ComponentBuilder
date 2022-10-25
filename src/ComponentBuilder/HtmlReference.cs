namespace ComponentBuilder;

/// <summary>
/// Represents a reference captured from <see cref="RenderTreeBuilder"/> for element or components.
/// </summary>
public class HtmlReference
{
    /// <summary>
    /// Gets the instance of reference captured by <see cref="RenderTreeBuilder"/>.
    /// </summary>
    public OneOf<ElementReference,object>? Reference { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HtmlReference"/> class.
    /// </summary>
    /// <param name="reference">The reference of element or component.</param>
    public HtmlReference(OneOf<ElementReference, object>? reference)
    {
        Reference = reference;
    }
}
