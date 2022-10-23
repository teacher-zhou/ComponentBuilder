namespace ComponentBuilder;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// 使用指定参数执行单击操作，并触发 <see cref="IHasOnClick{TEventArgs}.OnClick"/> 的回调。
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick{TEventArgs}"/>.</param>
    /// <param name="args">当鼠标单击后包含的参数。</param>
    /// <param name="before">在 <see cref="IHasOnClick{TEventArgs}.OnClick"/> 执行回调前调用的函数。</param>
    /// <param name="after">在 <see cref="IHasOnClick{TEventArgs}.OnClick"/> 执行回调后调用的函数。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    /// <returns>任务表示单击操作，没有返回结果。</returns>
    public static async Task Click<TEventArgs>(this IHasOnClick<TEventArgs?> clickEvent, TEventArgs? args = default, Func<TEventArgs?, Task>? before = default, Func<TEventArgs?, Task>? after = default, bool refresh = default)
    {
        before?.Invoke(args);
        await clickEvent.OnClick.InvokeAsync(args);
        after?.Invoke(args);
        await clickEvent.Refresh(refresh);
    }

    /// <summary>
    /// 执行单击操作，并触发 <see cref="IHasOnClick{MouseEventArgs}.OnClick"/> 的回调。
    /// </summary>
    /// <param name="clickEvent">Instanc of <see cref="IHasOnClick{MouseEventArgs}"/>.</param>
    /// <param name="args">当鼠标单击后包含的参数。</param>
    /// <param name="before">在 <see cref="IHasOnClick{MouseEventArgs}.OnClick"/> 执行回调前调用的函数。</param>
    /// <param name="after">在 <see cref="IHasOnClick{MouseEventArgs}.OnClick"/> 执行回调后调用的函数。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    /// <returns>任务表示单击操作，没有返回结果。</returns>
    public static Task Click(this IHasOnClick<MouseEventArgs?> clickEvent, MouseEventArgs? args = default, Func<MouseEventArgs?, Task>? before = default, Func<MouseEventArgs?, Task>? after = default, bool refresh = default)
        => clickEvent.Click<MouseEventArgs?>(args, before, after, refresh);

    /// <summary>
    /// 执行激活操作，并触发 <see cref="IHasOnActive.OnActive"/> 的回调。
    /// </summary>
    /// <param name="activeEvent">实现 <see cref="IHasOnActive"/> 的实例。</param>
    /// <param name="active">一个激活状态。</param>
    /// <param name="before">在 <see cref="IHasOnActive.OnActive"/> 执行回调前调用的函数。</param>
    /// <param name="after">在 <see cref="IHasOnActive.OnActive"/> 执行回调后调用的函数。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    /// <returns>任务表示单击操作，没有返回结果。</returns>
    public static async Task Activate(this IHasOnActive activeEvent, bool active = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(active);
        activeEvent.Active = active;
        await activeEvent.OnActive.InvokeAsync(active);
        after?.Invoke(active);

        await activeEvent.Refresh(refresh);
    }


    /// <summary>
    /// 执行禁用操作，并触发 <see cref="IHasOnDisabled.OnDisabled"/> 的回调。
    /// </summary>
    /// <param name="disabledEvent">实现 <see cref="IHasOnDisabled"/> 的实例。</param>
    /// <param name="disabled">一个禁用状态。</param>
    /// <param name="before">在 <see cref="IHasOnDisabled.OnDisabled"/> 执行回调前调用的函数。</param>
    /// <param name="after">在 <see cref="IHasOnDisabled.OnDisabled"/> 执行回调后调用的函数。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    /// <returns>任务表示单击操作，没有返回结果。</returns>
    public static async Task Disable(this IHasOnDisabled disabledEvent, bool disabled = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(disabled);
        disabledEvent.Disabled = disabled;
        await disabledEvent.OnDisabled.InvokeAsync(disabled);
        after?.Invoke(disabled);
        await disabledEvent.Refresh(refresh);
    }

    /// <summary>
    /// 执行一个函数来切换组件集合中的指定索引项。
    /// </summary>
    /// <param name="instance">Instanc of <see cref="IHasOnSwitch"/>.</param>
    /// <param name="index">在组件中切换的索引。设置 <c>null</c> 可以清空切换。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    /// <returns>任务表示单击操作，没有返回结果。</returns>
    public static async Task SwitchTo(this IHasOnSwitch instance, int? index = default, bool refresh = true)
    {
        instance.SwitchIndex = index;
        await instance.OnSwitch.InvokeAsync(index);

        if (instance is BlazorAbstractComponentBase component)
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
    /// 以异步的方式通知组件的状态已经改变并需要立即刷新重新渲染。
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="refresh"><c>true</c> 立即刷新渲染。</param>
    /// <returns>任务表示单击操作，没有返回结果。</returns>
    public static Task Refresh(this IRefreshableComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }
}
