namespace ComponentBuilder;

public static class ParameterExtensions
{
    public static async Task Click(this IHasOnClick click, Action beforeClick, Action afterClick = default)
    {
        beforeClick();
        await click.OnClick.InvokeAsync();
        afterClick?.Invoke();
    }
}
