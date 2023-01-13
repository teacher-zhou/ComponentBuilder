using ComponentBuilder.Interceptors;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace ComponentBuilder.Demo.ServerSide
{
    public class LogInterceptor : ComponentInterceptorBase
    {
        public override void InterceptOnInitialized(IBlazorComponent component)
        {

        }
    }
}
