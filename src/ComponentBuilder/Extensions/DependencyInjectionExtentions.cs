using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;
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

        foreach (var resoverType in options.HtmlAttributeResolvers)
        {
            var serviceType = typeof(IHtmlAttributeResolver);

            if (!serviceType.IsAssignableFrom(resoverType))
            {
                throw new InvalidOperationException($"The resolver must implement from {serviceType.Name} interface");
            }
            services.AddTransient(serviceType, resoverType);
        }

        foreach ( var resoverType in options.CssClassResolvers )
        {
            var serviceType = typeof(ICssClassResolver);

            if ( !serviceType.IsAssignableFrom(resoverType) )
            {
                throw new InvalidOperationException($"The resolver must implement from {serviceType.Name} interface");
            }
            services.AddTransient(serviceType, resoverType);
        }

        foreach (var interceptorType in options.Interceptors)
        {
            var serviceType = typeof(IComponentInterceptor);
            if (!serviceType.IsAssignableFrom(interceptorType))
            {
                throw new InvalidOperationException($"The interceptor must implement from {serviceType.Name} interface");
            }

            services.AddTransient(serviceType, interceptorType);
        }

        if ( !options.Renders.Contains(typeof(DefaultComponentRender)) )
        {
            options.Renders.Insert(options.Renders.Count, typeof(DefaultComponentRender));
        }

        foreach ( var renderType in options.Renders )
        {
            var serviceType = typeof(IComponentRender);
            if ( !serviceType.IsAssignableFrom(renderType) )
            {
                throw new InvalidOperationException($"The renderer of component must implement from {serviceType.Name} interface");
            }

            services.AddTransient(serviceType, renderType);
        }



        services
            .AddTransient<ICssClassResolver, CssClassAttributeResolver>()
            .AddTransient<HtmlTagAttributeResolver>()
            ;

        return services;
    }
}
