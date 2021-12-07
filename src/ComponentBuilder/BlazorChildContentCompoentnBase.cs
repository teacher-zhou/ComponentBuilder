namespace ComponentBuilder;

/// <summary>
/// Represents an abstract blazor component class that has child content.
/// </summary>
public abstract class BlazorChildContentCompoentnBase : BlazorComponentBase, IBlazorChildContentComponent
{
    /// <summary>
    /// A segment of UI content.
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
}

/// <summary>
/// Represents an abstract blazor component class that has child content.
/// </summary>
public abstract class BlazorChildContentCompoentnBase<TValue> : BlazorComponentBase, IBlazorChildContentComponent<TValue>
{
    /// <summary>
    /// A segment of UI content for an object of type.
    /// </summary>
    [Parameter] public RenderFragment<TValue>? ChildContent { get; set; }
}
