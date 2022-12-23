namespace ComponentBuilder;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// Performs a click with the specified parameters and fires the callback. 
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick{TEventArgs}"/>.</param>
    /// <param name="args">Parameters that are included when the mouse is clicked.</param>
    /// <param name="before">Performs an action before <see cref="IHasOnClick{TEventArgs}.OnClick"/> invoke.</param>
    /// <param name="after">Performs an action after <see cref="IHasOnClick{TEventArgs}.OnClick"/> invoke.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static async Task Click<TEventArgs>(this IHasOnClick<TEventArgs?> clickEvent, TEventArgs? args = default, Func<TEventArgs?, Task>? before = default, Func<TEventArgs?, Task>? after = default, bool refresh = default)
    {
        before?.Invoke(args);
        await clickEvent.OnClick.InvokeAsync(args);
        after?.Invoke(args);
        await clickEvent.Refresh(refresh);
    }

    /// <summary>
    /// Performs a click with the specified parameters and fires the callback. 
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick{TEventArgs}"/>.</param>
    /// <param name="args">Parameters that are included when the mouse is clicked.</param>
    /// <param name="before">Performs an action before <see cref="IHasOnClick{TEventArgs}.OnClick"/> invoke.</param>
    /// <param name="after">Performs an action after <see cref="IHasOnClick{TEventArgs}.OnClick"/> invoke.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static Task Click(this IHasOnClick<MouseEventArgs?> clickEvent, MouseEventArgs? args = default, Func<MouseEventArgs?, Task>? before = default, Func<MouseEventArgs?, Task>? after = default, bool refresh = default)
        => clickEvent.Click<MouseEventArgs?>(args, before, after, refresh);

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
    /// <param name="instance">Instanc of <see cref="IHasOnSwitch"/>.</param>
    /// <param name="index">The index to switch between components. Set <c>null</c> clears the switch.</param>
    /// <param name="refresh">Notifies the component that the state has changed and refreshes immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static async Task SwitchTo(this IHasOnSwitch instance, int? index = default, bool refresh = true)
    {
        instance.SwitchIndex = index;
        await instance.OnSwitch.InvokeAsync(index);

        if (instance is BlazorComponentBase component)
        {
            for (int i = 0; i < component.ChildComponents.Count; i++)
            {
                var childComponent = component.ChildComponents[i];

                if (childComponent is IHasActive activeComponent)
                {
                    activeComponent.Active = false;
                }
            }

            if (index.HasValue && index >= 0)
            {
                var childComponent = component.ChildComponents[index.Value];
                if (childComponent is IHasActive activeComponent)
                {
                    activeComponent.Active = true;
                }
                if (childComponent is IHasOnActive onActiveComponent)
                {
                    await onActiveComponent.OnActive.InvokeAsync(true);
                }
            }
            await instance.Refresh(refresh);
        }
    }

    /// <summary>
    /// Asynchronously notifies a component that its state has changed and that it needs to be refreshed and re-rendered immediately.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="refresh"><c>true</c> to notify the component state has changed immediately.</param>
    /// <returns>A task contains avoid return value.</returns>
    public static Task Refresh(this IRefreshableComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }
}
