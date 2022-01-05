namespace ComponentBuilder;
/// <summary>
/// Represents a base component for child associated with <see cref="BlazorParentComponentBase{TParentComponent, TChildComponent}"/> class.
/// </summary>
/// <typeparam name="TParentComponent">The parent component type.</typeparam>
public abstract class BlazorChildComponentBase<TParentComponent> : BlazorChildContentComponentBase
    where TParentComponent : ComponentBase
{
    /// <summary>
    /// Gets instance of parent component.
    /// </summary>
    [CascadingParameter] protected TParentComponent ParentComponent { get; private set; }


    /// <summary>
    /// Overried to validate and throw exception when <see cref="ParentComponent"/> is <c>null</c> value.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        ThrowIfParentComponentNull();
        await base.OnInitializedAsync();
    }

    /// <summary>
    /// Throws an exception when <see cref="ParentComponent"/> is <c>null</c> value.
    /// </summary>
    /// <exception cref="InvalidOperationException">This component must be the child of <see cref="ParentComponent"/> component.</exception>
    protected virtual void ThrowIfParentComponentNull()
    {
        if (ParentComponent is null)
        {
            throw new InvalidOperationException($"The '{GetType().Name}' component must be the child of '{typeof(TParentComponent).Name}' component");
        }
    }
}
/// <summary>
/// Represents a base component for child associated with <see cref="BlazorParentComponentBase{TParentComponent, TChildComponent}"/> class.
/// </summary>
/// <typeparam name="TParentComponent">The parent component type.</typeparam>
/// <typeparam name="TChildComponent">The child component type.</typeparam>
public abstract class BlazorChildComponentBase<TParentComponent, TChildComponent> : BlazorChildComponentBase<TParentComponent>
    where TParentComponent : BlazorParentComponentBase<TParentComponent, TChildComponent>
    where TChildComponent : ComponentBase
{

    /// <summary>
    /// Overried to validate and throw exception when parent component is <c>null</c> value.
    /// </summary>
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await ParentComponent.AddChildComponent(this);
    }

    /// <summary>
    /// Throws an exception when parent component is <c>null</c> value.
    /// </summary>
    /// <exception cref="InvalidOperationException">This component must be the child of parent component.</exception>
    protected override void ThrowIfParentComponentNull()
    {
        if (ParentComponent is null)
        {
            throw new InvalidOperationException($"The '{typeof(TChildComponent).Name}' component must be the child of '{typeof(TParentComponent).Name}' component");
        }
    }
}
