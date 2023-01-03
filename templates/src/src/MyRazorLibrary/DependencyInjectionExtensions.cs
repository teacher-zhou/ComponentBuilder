namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Change you name of extensions.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddMyRazorLibrary(this IServiceCollection services)
    {
        return services.AddComponentBuilder();
    }
}
