using ComponentBuilder.Automation.Definitions;

namespace ComponentBuilder.Automation;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// Performs a click with the specified parameters and fires the callback. 
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick"/>.</param>
    /// <param name="args">Parameters that are included when the mouse is clicked.</param>
    /// <param name="before">Performs an action before <see cref="IHasOnClick.OnClick"/> invoke.</param>
    /// <param name="after">Performs an action after <see cref="IHasOnClick.OnClick"/> invoke.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static async Task Click(this IHasOnClick clickEvent, MouseEventArgs? args = default, Func<MouseEventArgs?, Task>? before = default, Func<MouseEventArgs?, Task>? after = default, bool refresh = default)
    {
        before?.Invoke(args);
        await clickEvent.OnClick.InvokeAsync(args);
        after?.Invoke(args);
        await clickEvent.Refresh(refresh);
    }

    /// <summary>
    /// Performs an activate action with the specified parameters and fires the callback.
    /// </summary>
    /// <param name="activeEvent">The instance of <see cref="IHasOnActive"/>.</param>
    /// <param name="active">A status to active.</param>
    /// <param name="before">Performs an action before <see cref="IHasOnActive.OnActive"/> invoke.</param>
    /// <param name="after">Performs an action after <see cref="IHasOnActive.OnActive"/> invoke.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static async Task Activate(this IHasOnActive activeEvent, bool active = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(active);
        activeEvent.Active = active;
        await activeEvent.OnActive.InvokeAsync(active);
        after?.Invoke(active);

        await activeEvent.Refresh(refresh);
    }

    /// <summary>
    /// Performs an disable action with the specified parameters and fires the callback.
    /// </summary>
    /// <param name="disabledEvent">The instance of <see cref="IHasOnDisabled"/>.</param>
    /// <param name="disabled">A status to disable.</param>
    /// <param name="before">Performs an action before <see cref="IHasOnDisabled.OnDisabled"/> invoke.</param>
    /// <param name="after">Performs an action after <see cref="IHasOnDisabled.OnDisabled"/> invoke.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static async Task Disable(this IHasOnDisabled disabledEvent, bool disabled = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(disabled);
        disabledEvent.Disabled = disabled;
        await disabledEvent.OnDisabled.InvokeAsync(disabled);
        after?.Invoke(disabled);
        await disabledEvent.Refresh(refresh);
    }

    /// <summary>
    /// Executes a function to switch a specified index item in a component collection.
    /// </summary>
    /// <param name="component">Instanc of <see cref="IHasOnSwitch"/>.</param>
    /// <param name="index">The index to switch between components. Set <c>null</c> clears the switch.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static async Task SwitchTo(this IHasOnSwitch component, int? index = default, bool refresh = true)
    {
        component.SwitchIndex = index;
        await component.OnSwitch.InvokeAsync(index);

        for ( int i = 0; i < component.ChildComponents.Count; i++ )
        {
            var childComponent = component.ChildComponents[i];

            if ( childComponent is IHasActive activeComponent )
            {
                activeComponent.Active = false;
            }
        }

        if ( index.HasValue && index >= 0 )
        {
            var childComponent = component.ChildComponents[index.Value];
            if ( childComponent is IHasActive activeComponent )
            {
                activeComponent.Active = true;
            }
            if ( childComponent is IHasOnActive onActiveComponent )
            {
                await onActiveComponent.OnActive.InvokeAsync(true);
            }
        }
        await component.Refresh(refresh);
    }

    /// <summary>
    /// Asynchronously notifies a component that its state has changed and that it needs to be refreshed and re-rendered immediately.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="refresh"><c>true</c> to notify the component state has changed immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static Task Refresh(this IBlazorComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// Determines whether the specified fields in <see cref="IHasEditContext.EditContext"/> has been modified.
    /// </summary>
    /// <param name="instance">the component instance.</param>
    /// <param name="fieldIdentifier">The fields in <see cref="IHasEditContext.EditContext"/>.</param>
    /// <param name="valid">Returns a boolean value represents the validation of <see cref="IHasEditContext.EditContext"/> is valid.</param>
    /// <returns>True if the field has been modified; otherwise false.</returns>
    /// <exception cref="ArgumentNullException">The <see cref="IHasEditContext.EditContext"/> is null.</exception>
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
    /// Determines whether any of the fields in <see cref="IHasEditContext.EditContext"/> has been modified.
    /// </summary>
    /// <param name="instance">the component instance.</param>
    /// <param name="valid">Returns a boolean value represents the validation of <see cref="IHasEditContext.EditContext"/> is valid.</param>
    /// <returns>True if any of fields in <see cref="IHasEditContext.EditContext"/> has been modified; otherwise false.</returns>
    /// <exception cref="ArgumentNullException">The <see cref="IHasEditContext.EditContext"/> is null.</exception>
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
