using ComponentBuilder.Definitions;

namespace ComponentBuilder;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    /// <summary>
    /// 使用指定的参数执行激活操作并触发回调。
    /// </summary>
    /// <param name="instance">实现 <see cref="IHasOnActive"/> 接口的实例。</param>
    /// <param name="active">激活状态。</param>
    /// <param name="before">在 <see cref="IHasOnActive.OnActive"/> 执行之前的委托。</param>
    /// <param name="after">在 <see cref="IHasOnActive.OnActive"/> 执行之后的委托。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    public static async Task Activate(this IHasOnActive instance, bool active = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(active);
        instance.Active = active;
        await instance.OnActive.InvokeAsync(active);
        after?.Invoke(active);

        await instance.Refresh(refresh);
    }

    /// <summary>
    /// 使用指定的参数执行禁用操作并触发回调。
    /// </summary>
    /// <param name="instance">实现 <see cref="IHasOnDisabled"/> 接口的实例。</param>
    /// <param name="disabled">禁用状态。</param>
    /// <param name="before">在 <see cref="IHasOnDisabled.OnDisabled"/> 执行之前的委托。</param>
    /// <param name="after">在 <see cref="IHasOnDisabled.OnDisabled"/> 执行之后的委托。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
    public static async Task Disable(this IHasOnDisabled instance, bool disabled = true, Func<bool, Task>? before = default, Func<bool, Task>? after = default, bool refresh = true)
    {
        before?.Invoke(disabled);
        instance.Disabled = disabled;
        await instance.OnDisabled.InvokeAsync(disabled);
        after?.Invoke(disabled);
        await instance.Refresh(refresh);
    }

    /// <summary>
    /// 执行函数以切换组件集合中的指定索引项。
    /// </summary>
    /// <param name="instance">实现 <see cref="IHasOnSwitch"/> 接口的实例。</param>
    /// <param name="index">在组件之间切换的索引。设置 <c>null</c> 清空开关。</param>
    /// <param name="refresh">通知组件状态已更改并立即刷新。</param>
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
    /// 异步地通知组件它的状态已经改变，需要立即刷新和重新呈现。
    /// </summary>
    /// <param name="component">The component.</param>
    /// <param name="refresh">设置 <c>true</c> 通知组件状态已经立即改变。</param>
    public static Task Refresh(this IBlazorComponent component, bool refresh = true)
    {
        if (refresh)
        {
            return component.NotifyStateChanged();
        }
        return Task.CompletedTask;
    }

    /// <summary>
    /// 确定指定的字段是否在 <see cref="IHasEditContext.EditContext"/> 已修改。
    /// </summary>
    /// <param name="instance">实现 <see cref="IHasEditContext"/> 接口的组件。</param>
    /// <param name="fieldIdentifier">要改变的字段。</param>
    /// <param name="valid">返回一个布尔值，表示字段的验证是合法的。</param>
    /// <returns>如果指定字段被修改，则为 <c>true</c>，否则返回 <c>false</c>。</returns>
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
    /// 确定任意字段是否在 <see cref="IHasEditContext.EditContext"/> 中被修改。
    /// </summary>
    /// <param name="instance">实现 <see cref="IHasEditContext"/> 接口的组件。</param>
    /// <param name="valid">返回一个布尔值，表示字段的验证是合法的。</param>
    /// <returns>如果任意的字段被修改，则为 <c>true</c>，否则返回 <c>false</c>。</returns>
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
