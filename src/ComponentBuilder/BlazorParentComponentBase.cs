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
    /// Represents a index of child component can be actived intially. The value must grater than -1.
    /// </summary>
    [Parameter] public virtual int ActiveIndex { get; set; }

    /// <summary>
    /// Perform an action when child component item with speified index is active.
    /// </summary>
    [Parameter] public virtual EventCallback<int> OnItemActive { get; set; }

    /// <summary>
    /// Add speicified component to be child of this component and refresh parent component.
    /// </summary>
    /// <param name="childComponent">The child component.</param>
    /// <returns>A task represents index of child component in collection.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="childComponent"/> is null.</exception>
    public virtual async Task<int> AddChildComponent(ComponentBase childComponent)
    {
        if (childComponent is null)
        {
            throw new ArgumentNullException(nameof(childComponent));
        }

        _childrenComponents.Add((TChildComponent)childComponent);
        var lastChildComponentIndex = _childrenComponents.Count - 1;
        if (ActiveIndex > -1 && ActiveIndex <= lastChildComponentIndex)
        {
            await ActiveChildComponent(ActiveIndex);
            NotifyStateChanged();
        }
        return lastChildComponentIndex;
    }

    private async Task ActiveChildComponent(int index)
    {
        var component = _childrenComponents[index];
        if (component is IHasActive childActivedComponent)
        {
            childActivedComponent.Active = true;
        }

        if (component is IHasOnActive activeEventComponent)
        {
            await activeEventComponent.OnActive.InvokeAsync(true);
        }
        if (OnItemActive.HasDelegate)
        {
            await OnItemActive.InvokeAsync(index);
        }
    }

    /// <summary>
    /// Active specified index of child component to active status. Before this action, all child component witch has implemented <see cref="IHasActive"/> should be set <see cref="IHasActive.Active"/> to <c>false</c>. The method will call <see cref="IRefreshComponent.NotifyStateChanged"/> function.
    /// </summary>
    /// <param name="index">The index of child component. If the value is less than 0 means no component specified.</param>
    /// <returns><c>true</c> actived child component successfully, otherwise, <c>false</c>. If child component does not implement from <see cref="IHasActive"/> interface or <paramref name="index"/> is less than 0, it always returns <c>false</c>.</returns>
    public virtual async Task Active(int index)
    {
        foreach (var item in _childrenComponents)
        {
            if (item is IHasActive activeComponent)
            {
                activeComponent.Active = false;
            }
        }

        if (index > -1)
        {
            await ActiveChildComponent(index);
            NotifyStateChanged();
        }
    }
}
