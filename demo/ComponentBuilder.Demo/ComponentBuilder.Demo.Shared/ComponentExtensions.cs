using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder.Components.Demo.Shared;
public static class Extensions
{
    public static IServiceCollection AddDemo(this IServiceCollection services, Action<RouterOptions> configure)
    {
        return services.Configure(configure);
    }
}
