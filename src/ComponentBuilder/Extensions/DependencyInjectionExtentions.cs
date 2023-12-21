using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder;

/// <summary>
/// The extension of DI.
/// </summary>
public static class DependencyInjectionExtentions
{
    /// <summary>
    /// Add Component Builder Framework Service with specified <see cref="ComponentConfigurationBuilder"/> action to  <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add.</param>
    /// <param name="configure">An action to configure <see cref="ComponentConfigurationBuilder"/> instace.</param>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services, Action<ComponentConfigurationBuilder> configure)
    {
        var builder = new ComponentConfigurationBuilder(services);
        
        configure?.Invoke(builder);

        services.AddTransient<ICssClassBuilder,DefaultCssClassBuilder>();
        services.AddTransient<IStyleBuilder, DefaultStyleBuilder>();

        return services;
    }

    /// <summary>
    /// Add Component Builder Framework Service with default configurations to  <see cref="IServiceCollection"/>.
    /// </summary>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
        => services.AddComponentBuilder(configure => configure.AddDefaultConfigurations());

    /// <summary>
    /// 为 <see cref="ComponentConfigurationBuilder"/> 实例添加默认解析器、渲染器和拦截器。
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultConfigurations(this ComponentConfigurationBuilder builder)
        => builder.AddDefaultResolvers().AddDefaultRenderers().AddDefaultInterceptors();

    static ComponentConfigurationBuilder AddDefaultResolvers(this ComponentConfigurationBuilder builder)
        => builder.AddResolver<IParameterClassResolver, CssClassAttributeResolver>()
        .AddResolver<IHtmlAttributeResolver, HtmlAttributeAttributeResolver>()
        .AddResolver<IHtmlTagAttributeResolver, HtmlTagAttributeResolver>()
        ;

    static ComponentConfigurationBuilder AddDefaultInterceptors(this ComponentConfigurationBuilder builder)
        => builder.AddInterceptor<ChildContentInterceptor>()
        .AddInterceptor<AssociationComponentInterceptor>()
        .AddInterceptor<FormComponentInterceptor>()
        .AddInterceptor<CssClassAttributeInterceptor>()
        .AddInterceptor<StyleAttributeInterceptor>()
        ;

    static ComponentConfigurationBuilder AddDefaultRenderers(this ComponentConfigurationBuilder builder)
        => builder.AddRenderer<DefaultComponentRender>()
        .AddRenderer<NavLinkComponentRender>();
}
