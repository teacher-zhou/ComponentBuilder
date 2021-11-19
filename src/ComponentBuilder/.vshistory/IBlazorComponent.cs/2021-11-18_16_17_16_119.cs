using Microsoft.AspNetCore.Components;

namespace ComponentBuilder
{
    public interface IBlazorComponent : IComponent
    {
        void StateHasChanged();
    }
}
