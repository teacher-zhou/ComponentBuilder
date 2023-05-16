using ComponentBuilder.Automation.Builder;
using ComponentBuilder.Automation.Interceptors;
using ComponentBuilder.Automation.Rendering;
using ComponentBuilder.Automation.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder.Automation;

/// <summary>
/// The extensions of DI for ComponentBuilder.Automation.
/// </summary>
public static class DependencyInjectionExtentions
{
    /// <summary>
    /// Add specified configuration for <see cref="ComponentConfigurationBuilder"/> instance to <see cref="IServiceCollection"/> instance.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add.</param>
    /// <param name="configure">An action to configure the builder.</param>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services, Action<ComponentConfigurationBuilder> configure)
    {
        var options = new ComponentBuilderOptions();
        var builder = new ComponentConfigurationBuilder(options, services);
        
        configure?.Invoke(builder);
        services.AddSingleton(options);

        services.AddTransient<ICssClassBuilder,DefaultCssClassBuilder>();
        services.AddTransient<IStyleBuilder, DefaultStyleBuilder>();

        return services;
    }

    /// <summary>
    /// Add default configurations for <see cref="ComponentConfigurationBuilder"/> instance.
    /// </summary>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
        => services.AddComponentBuilder(configure => configure.AddDefaultConfigurations());

    /// <summary>
    /// Add default resolvers, renderers, interceptors for <see cref="ComponentConfigurationBuilder"/> instance.
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultConfigurations(this ComponentConfigurationBuilder builder)
        => builder.AddDefaultResolvers().AddDefaultRenderers().AddDefaultInterceptors();

    /// <summary>
    /// Add default resolvers to <see cref="ComponentConfigurationBuilder"/> class.
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultResolvers(this ComponentConfigurationBuilder builder)
        => builder.AddResolver<ICssClassResolver, CssClassAttributeResolver>()
        .AddResolver<IHtmlAttributeResolver, HtmlAttributeAttributeResolver>()
        .AddResolver<IHtmlTagAttributeResolver, HtmlTagAttributeResolver>()
        ;

    /// <summary>
    /// Add default interceptors to <see cref="ComponentConfigurationBuilder"/> class.
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultInterceptors(this ComponentConfigurationBuilder builder)
        => builder.AddInterceptor<ChildContentInterceptor>()
        .AddInterceptor<AssociationComponentInterceptor>()
        .AddInterceptor<FormComponentInterceptor>()
        .AddInterceptor<CssClassAttributeInterceptor>()
        .AddInterceptor<StyleAttributeInterceptor>()
        ;

    /// <summary>
    /// Add default renderers to <see cref="ComponentConfigurationBuilder"/> class.
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultRenderers(this ComponentConfigurationBuilder builder)
        => builder.AddRenderer<DefaultComponentRender>()
        .AddRenderer<NavLinkComponentRender>();
}
