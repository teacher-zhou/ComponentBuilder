using ComponentBuilder.Abstrations;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent
    {
        [Inject] protected ICssClassBuilder CssBuilder { get; set; }

        public string BuildCssClassString()
        {
            throw new System.NotImplementedException();
        }

        public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
    }
}
