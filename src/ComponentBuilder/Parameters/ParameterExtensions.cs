namespace ComponentBuilder.Parameters;

/// <summary>
/// The extensions of parameters.
/// </summary>
public static class ParameterExtensions
{
    public static async Task Click(this IHasOnClick clickEvent, MouseEventArgs args = default, bool stateChanged = default)
    {
        await clickEvent.OnClick.InvokeAsync(args);
        if (stateChanged)
        {
            await clickEvent.NotifyStateChanged();
        }
    }

    public static async Task Active(this IHasOnActived activedEvent, bool active = true)
    {
        activedEvent.Active = active;
        await activedEvent.OnActived.InvokeAsync(active);
        await activedEvent.NotifyStateChanged();
    }

    public static async Task Disable(this IHasOnDisabled disabledEvent, bool disabled = true)
    {
        disabledEvent.Disabled = disabled;
        await disabledEvent.OnDisabled.InvokeAsync(disabled);
        await disabledEvent.NotifyStateChanged();
    }
}
