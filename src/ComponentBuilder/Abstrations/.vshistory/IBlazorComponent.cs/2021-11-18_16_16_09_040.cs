using Microsoft.AspNetCore.Components;

namespace ComponentBuilder.Abstrations
{
    public interface IBlazorComponent : IComponent
    {
        void StateHasChanged();
    }
}
