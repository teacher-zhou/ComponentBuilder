using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    public interface IBlazorComponent : IComponent
    {
        Task NotifyStateChanged();

        string BuildCssClassString();
    }
}
