using ComponentBuilder.Abstrations.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder;

/// <summary>
/// The extensions of DI for ComponentBuilder.
/// </summary>
public static class DependencyInjectionExtentions
{
    /// <summary>
    /// Add default serivces of ComponentBuilder to <see cref="IServiceCollection"/> instance.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add.</param>
    /// <param name="configure">An action to configure the options.</param>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services, Action<ComponentBuilderOptions>? configure = default)
    {
        var options = new ComponentBuilderOptions();

        configure?.Invoke(options);

        services.AddTransient(provider => options.ClassBuilder ?? new DefaultCssClassBuilder());
        services.AddTransient(provider => options.StyleBuilder ?? new DefaultStyleBuilder());
        
        foreach(var htmlResolver in options.Resolvers)
        {
            services.AddTransient(provider => htmlResolver);
        }

        foreach ( var interceptor in options.Interceptors )
        {
            services.AddTransient(provider => interceptor);
        }

        services
            .AddTransient<ICssClassAttributeResolver, CssClassAttributeResolver>()
            .AddTransient<HtmlTagAttributeResolver>()
            ;

        return services;
    }
}
