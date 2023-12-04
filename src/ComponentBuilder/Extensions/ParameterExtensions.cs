namespace ComponentBuilder;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// Performs the activation operation with the specified parameters and triggers the callback.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="active">Active state.</param>
    /// <param name="before">Call function before <see cref="IHasOnActive.OnActive"/> .</param>
    /// <param name="after">Call after <see cref="IHasOnActive.OnActive"/> .</param>
    /// <param name="refresh">Notifies the component that its state has changed and refreshes immediately.</param>
    public static async Task Activate(this IHasOnActive instance, bool active = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(active);
        instance.Active = active;
        await instance.OnActive.InvokeAsync(active);
        after?.Invoke(active);

        await instance.Refresh(refresh);
    }

    /// <summary>
    /// Performs the disable action and triggers the callback with the specified parameters.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="disabled">Disable state.</param>
    /// <param name="before">Call function before <see cref="IHasOnDisabled.OnDisabled"/> .</param>
    /// <param name="after">Call after <see cref="IHasOnDisabled.OnDisabled"/> .</param>
    /// <param name="refresh">Notifies the component that its state has changed and refreshes immediately.</param>
    public static async Task Disable(this IHasOnDisabled instance, bool disabled = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(disabled);
        instance.Disabled = disabled;
        await instance.OnDisabled.InvokeAsync(disabled);
        after?.Invoke(disabled);
        await instance.Refresh(refresh);
    }

    /// <summary>
    /// Executes a function to toggle the specified index item in the component collection.
    /// </summary>
    /// <param name="instance"></param>
    /// <param name="index">Index to switch between components. Set the <c>null</c> empty switch.</param>
    /// <param name="refresh">Notifies the component that its state has changed and refreshes immediately.</param>
    public static async Task SwitchTo(this IHasOnSwitch instance, int? index = default, bool refresh = true)
    {
        instance.SwitchIndex = index;
        await instance.OnSwitch.InvokeAsync(index);

        for ( int i = 0; i < instance.ChildComponents.Count; i++ )
        {
            var childComponent = instance.ChildComponents[i];

            if ( childComponent is IHasActive activeComponent )
            {
                activeComponent.Active = false;
            }
        }

        if ( index.HasValue && index >= 0 )
        {
            var childComponent = instance.ChildComponents[index.Value];
            if ( childComponent is IHasActive activeComponent )
            {
                activeComponent.Active = true;
            }
            if ( childComponent is IHasOnActive onActiveComponent )
            {
                await onActiveComponent.OnActive.InvokeAsync(true);
            }
        }
        await instance.Refresh(refresh);
    }

    /// <summary>
    /// Asynchronously notifies the component that its state has changed and needs to be refreshed and re-rendered immediately.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="refresh">Set <c>true</c> to notify that the component state has changed immediately.</param>
    public static Task Refresh(this IBlazorComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// To determine whether the fields specified in the <see  cref="IHasEditContext. EditContext" /> has been modified.
    /// </summary>
    /// <param name="instance"> <see cref="IHasEditContext"/></param>
    /// <param name="fieldIdentifier">The field to change.</param>
    /// <param name="valid">Returns a Boolean value indicating that the validation of the field is valid.</param>
    /// <returns><c>true</c> if the specified field is modified, otherwise <c>false</c> is returned.</returns>
    /// <exception cref="ArgumentNullException"><see cref="IHasEditContext.EditContext"/> 是 null.</exception>
    public static bool IsModified(this IHasEditContext instance, in FieldIdentifier fieldIdentifier, out bool valid)
    {
        if ( instance.EditContext is null )
        {
            throw new InvalidOperationException($"{nameof(instance.EditContext)} cannot be null");
        }

        var modified = instance.EditContext.IsModified(fieldIdentifier);
        valid = !instance.EditContext.GetValidationMessages().Any();
        return modified;
    }

    /// <summary>
    /// To determine whether any field in <see cref = "IHasEditContext. EditContext" /> has been modified.
    /// </summary>
    /// <param name="instance"><see cref="IHasEditContext"/> </param>
    /// <param name="valid">Returns a Boolean value indicating that the validation of the field is valid.</param>
    /// <returns><c>true</c> if the specified field is modified, otherwise <c>false</c> is returned.</returns>
    /// <exception cref="ArgumentNullException"><see cref="IHasEditContext.EditContext"/> 是 null.</exception>
    public static bool IsModified(this IHasEditContext instance, out bool valid)
    {
        if ( instance.EditContext is null )
        {
            throw new InvalidOperationException($"{nameof(instance.EditContext)} cannot be null");
        }

        var modified = instance.EditContext.IsModified();
        valid = !instance.EditContext.GetValidationMessages().Any();
        return modified;
    }

}
