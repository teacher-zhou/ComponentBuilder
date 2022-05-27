namespace ComponentBuilder;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// Performs click action and <see cref="IHasOnClick.OnClick"/> will be invoked.
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick"/>.</param>
    /// <param name="args">Event arguments representing mouse has clicked.</param>
    /// <param name="before">An action performs before <see cref="IHasOnClick.OnClick"/> invokes.</param>
    /// <param name="after">An action performs after <see cref="IHasOnClick.OnClick"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents a click action and no result to return.</returns>
    public static async Task Click(this IHasOnClick clickEvent, MouseEventArgs args = default, Action<MouseEventArgs> before = default, Action<MouseEventArgs> after = default, bool refresh = default)
    {
        before?.Invoke(args);
        await clickEvent.OnClick.InvokeAsync(args);
        after?.Invoke(args);

        await clickEvent.Refresh(refresh);
    }

    /// <summary>
    /// Performs an activate action and <see cref="IHasOnActive.OnActive"/> will be invoked.
    /// </summary>
    /// <param name="activeEvent">Instanc of <see cref="IHasOnActive"/>.</param>
    /// <param name="active">A status to active.</param>
    /// <param name="before">An action performs before <see cref="IHasOnActive.OnActive"/> invokes.</param>
    /// <param name="after">An action performs after <see cref="IHasOnActive.OnActive"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents an active action and no result to return.</returns>
    public static async Task Activate(this IHasOnActive activeEvent, bool active = true, Action<bool> before = default, Action<bool> after = default, bool refresh = true)
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
    /// <param name="before">An action performs before <see cref="IHasOnDisabled.OnDisabled"/> invokes.</param>
    /// <param name="after">An action performs after <see cref="IHasOnDisabled.OnDisabled"/> has invoked.</param>
    /// <param name="refresh">Notify component that state has changed and refresh immediately.</param>
    /// <returns>A task represents a disable action and no result to return.</returns>
    public static async Task Disable(this IHasOnDisabled disabledEvent, bool disabled = true, Action<bool> before = default, Action<bool> after = default, bool refresh = true)
    {
        before?.Invoke(disabled);
        disabledEvent.Disabled = disabled;
        await disabledEvent.OnDisabled.InvokeAsync(disabled);
        after?.Invoke(disabled);
        await disabledEvent.Refresh(refresh);
    }

    #region 不工作，为何？
    /// <summary>
    /// Create a callback when value has changed.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="instance">The instance that value has changed.</param>
    /// <param name="existingValue">The current value to be changed.</param>
    /// <returns>A callback delegate for component with <see cref="ChangeEventArgs"/>.</returns>
    internal static EventCallback<ChangeEventArgs> CreateValueChangedBinder<TValue>(this IHasTwoWayBinding<TValue?> instance, object receiver, TValue? existingValue)
    {
        return HtmlHelper.CreateCallbackBinder<TValue?>(receiver, value => existingValue = value, existingValue);
    }
    #endregion

    public static async Task SwitchTo(this IHasOnSwitch instance, int? index, bool refresh = true)
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
    internal static Task Refresh(this IRefreshableComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }
}
