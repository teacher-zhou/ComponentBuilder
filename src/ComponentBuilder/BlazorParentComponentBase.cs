namespace ComponentBuilder;

/// <summary>
/// Represents a base parament component class associate to <see cref="BlazorChildComponentBase{TParentComponent}"/> class.
/// </summary>
/// <typeparam name="TParentComponent">The parent component type.</typeparam>
public abstract class BlazorParentComponentBase<TParentComponent> : BlazorChildContentComponentBase
    where TParentComponent : ComponentBase
{

    /// <summary>
    /// If <c>true</c>, indicates that <see cref="CascadingValue{TValue}.Value"/> will not change. This is a performance optimization that allows the framework to skip setting up change notifications.
    /// </summary>
    protected virtual bool IsFixed => false;
    /// <summary>
    /// Gets the name of cascading parameter.
    /// </summary>
    /// <value>value can be <c>null</c>.</value>
    protected virtual string? Name => null;


    /// <summary>
    /// Override to create cascading value for this component and continue building component tree.
    /// </summary>
    /// <param name="builder"></param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        this.CreateCascadingComponent<TParentComponent>(builder, 1, child =>
        {
            child.OpenElement(0, TagName);
            BuildComponentRenderTree(child);
            child.CloseElement();
        }, Name, IsFixed);
    }
}

/// <summary>
/// Represents a base parament component class associate to <see cref="BlazorChildComponentBase{TParentComponent,TChildComponent}"/> class.
/// </summary>
/// <typeparam name="TParentComponent">The parent component type.</typeparam>
/// <typeparam name="TChildComponent">The child component type.</typeparam>
public abstract class BlazorParentComponentBase<TParentComponent, TChildComponent> : BlazorParentComponentBase<TParentComponent>
    where TParentComponent : ComponentBase
    where TChildComponent : ComponentBase
{

    private readonly BlazorComponentCollection<TChildComponent> _childrenComponents = new();

    /// <summary>
    /// Gets child components is added.
    /// </summary>
    public BlazorComponentCollection<TChildComponent> ChildComponents => _childrenComponents;

    /// <summary>
    /// Add speicified component to be child of this component and refresh parent component.
    /// </summary>
    /// <param name="childComponent">The child component.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="childComponent"/> is null.</exception>
    public Task AddChildComponent(ComponentBase childComponent)
    {
        if (childComponent is null)
        {
            throw new ArgumentNullException(nameof(childComponent));
        }

        _childrenComponents.Add((TChildComponent)childComponent);
        return NotifyStateChanged();
    }
}
