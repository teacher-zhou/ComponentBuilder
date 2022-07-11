namespace ComponentBuilder;

/// <summary>
/// Represents an abstract blazor component class that has child content.
/// </summary>
public abstract class BlazorChildContentComponentBase : BlazorComponentBase, IHasChildContent
{
    /// <summary>
    /// A segment of UI content.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Appends frames representing an arbitrary fragment of content that has implemented from <see cref="IHasChildContent"/>.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        AddChildContent(builder, sequence);
    }
}

/// <summary>
/// Represents an abstract blazor component class that has child content with specified value.
/// </summary>
/// <typeparam name="TValue">The type of object.</typeparam>
public abstract class BlazorChildContentComponentBase<TValue> : BlazorComponentBase, IHasChildContent<TValue>
{
    /// <summary>
    /// A segment of UI content for an object of type <typeparamref name="TValue"/>.
    /// </summary>
    [Parameter] public RenderFragment<TValue>? ChildContent { get; set; }

    /// <summary>
    /// Returns <typeparamref name="TValue"/> for ChildContent.
    /// </summary>
    /// <returns>The type of value.</returns>
    protected abstract TValue GetChildContentValue();
    /// <summary>
    /// Appends frames representing an arbitrary fragment of content that has implemented from <see cref="IHasChildContent{TValue}"/>.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected override void AddContent(RenderTreeBuilder builder, int sequence)
    {
        AddChildContent(builder, sequence, GetChildContentValue());
    }
}
