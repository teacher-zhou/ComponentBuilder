namespace ComponentBuilder;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// Performs click action with specify argument and <see cref="IHasOnClick{TEventArgs}.OnClick"/> will be invoked.
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick{TEventArgs}"/>.</param>
    /// <param name="args">Event arguments representing mouse has clicked.</param>
    /// <param name="before">A function performs before <see cref="IHasOnClick{TEventArgs}.OnClick"/> invokes.</param>
    /// <param name="after">A function performs after <see cref="IHasOnClick{TEventArgs}.OnClick"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents a click action and no result to return.</returns>
    public static async Task Click<TEventArgs>(this IHasOnClick<TEventArgs?> clickEvent, TEventArgs? args = default, Func<TEventArgs?, Task>? before = default, Func<TEventArgs?, Task>? after = default, bool refresh = default)
    {
        before?.Invoke(args);
        await clickEvent.OnClick.InvokeAsync(args);
        after?.Invoke(args);
        await clickEvent.Refresh(refresh);
    }

    /// <summary>
    /// Performs click action with <see cref="MouseEventArgs"/> argument and <see cref="IHasOnClick{TEventArgs}.OnClick"/> will be invoked.
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick"/>.</param>
    /// <param name="args">Event arguments representing mouse has clicked.</param>
    /// <param name="before">A function performs before <see cref="IHasOnClick{TEventArgs}.OnClick"/> invokes.</param>
    /// <param name="after">A function performs after <see cref="IHasOnClick{TEventArgs}.OnClick"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents a click action and no result to return.</returns>
    public static Task Click(this IHasOnClick<MouseEventArgs?> clickEvent, MouseEventArgs? args = default, Func<MouseEventArgs?, Task>? before = default, Func<MouseEventArgs?, Task>? after = default, bool refresh = default) 
        => clickEvent.Click<MouseEventArgs?>(args, before, after, refresh);

    /// <summary>
    /// Performs an activate action and <see cref="IHasOnActive.OnActive"/> will be invoked.
    /// </summary>
    /// <param name="activeEvent">Instanc of <see cref="IHasOnActive"/>.</param>
    /// <param name="active">A status to active.</param>
    /// <param name="before">A function performs before <see cref="IHasOnActive.OnActive"/> invokes.</param>
    /// <param name="after">A function performs after <see cref="IHasOnActive.OnActive"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents an active action and no result to return.</returns>
    public static async Task Activate(this IHasOnActive activeEvent, bool active = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(active);
        activeEvent.Active = active;
        await activeEvent.OnActive.InvokeAsync(active);
        after?.Invoke(active);

        await activeEvent.Refresh(refresh);
    }


    /// <summary>
    /// Performs disable action and <see cref="IHasOnDisabled.OnDisabled"/> will be invoked.
    /// </summary>
    /// <param name="disabledEvent">Instanc of <see cref="IHasOnDisabled"/>.</param>
    /// <param name="disabled">A status to active.</param>
    /// <param name="before">A function performs before <see cref="IHasOnDisabled.OnDisabled"/> invokes.</param>
    /// <param name="after">A function performs after <see cref="IHasOnDisabled.OnDisabled"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents a disable action and no result to return.</returns>
    public static async Task Disable(this IHasOnDisabled disabledEvent, bool disabled = true, Func<bool,Task>? before = default, Func<bool,Task>? after = default, bool refresh = true)
    {
        before?.Invoke(disabled);
        disabledEvent.Disabled = disabled;
        await disabledEvent.OnDisabled.InvokeAsync(disabled);
        after?.Invoke(disabled);
        await disabledEvent.Refresh(refresh);
    }

    ///// <summary>
    ///// Create a callback when value has changed.
    ///// </summary>
    ///// <typeparam name="TValue">The type of value.</typeparam>
    ///// <param name="instance">The instance that value has changed.</param>
    ///// <param name="existingValue">The current value to be changed.</param>
    ///// <returns>A callback delegate for component with <see cref="ChangeEventArgs"/>.</returns>
    //internal static EventCallback<ChangeEventArgs> CreateValueChangedBinder<TValue>(this IHasTwoWayBinding<TValue?> instance, object receiver, TValue? existingValue)
    //{
    //    return HtmlHelper.CreateCallbackBinder<TValue?>(receiver, value => existingValue = value, existingValue);
    //}

    /// <summary>
    /// Perform a function to switch specify index item in component collection.
    /// </summary>
    /// <param name="instance">Instanc of <see cref="IHasOnSwitch"/>.</param>
    /// <param name="index">The index to switch in components. Set <c>null</c> to clear active item.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents a disable action and no result to return.</returns>
    public static async Task SwitchTo(this IHasOnSwitch instance, int? index=default, bool refresh = true)
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
    /// Notify component state has changed and refresh immediately.
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="refresh">Refresh immediately.</param>
    /// <returns>A task represents a refresh action and no result to return.</returns>
    public static Task Refresh(this IRefreshableComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }
}
