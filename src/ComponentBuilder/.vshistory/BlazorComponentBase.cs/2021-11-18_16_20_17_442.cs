using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent
    {
        [Inject] protected ICssClassBuilder CssBuilder { get; set; }

        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
    }
}
