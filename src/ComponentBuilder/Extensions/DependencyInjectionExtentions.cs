using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder;

/// <summary>
/// 依赖注入扩展。
/// </summary>
public static class DependencyInjectionExtentions
{
    /// <summary>
    /// 向 <see cref="IServiceCollection"/> 添加基于 <see cref="ComponentConfigurationBuilder"/> 的服务。
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add.</param>
    /// <param name="configure">配置构建器的操作。</param>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services, Action<ComponentConfigurationBuilder> configure)
    {
        var builder = new ComponentConfigurationBuilder(services);
        
        configure?.Invoke(builder);

        services.AddTransient<ICssClassBuilder,DefaultCssClassBuilder>();
        services.AddTransient<IStyleBuilder, DefaultStyleBuilder>();

        return services;
    }

    /// <summary>
    ///向 <see cref="IServiceCollection"/> 添加默认配置的服务。
    /// </summary>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
        => services.AddComponentBuilder(configure => configure.AddDefaultConfigurations());

    /// <summary>
    /// 为 <see cref="ComponentConfigurationBuilder"/> 实例添加默认解析器、渲染器和拦截器。
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultConfigurations(this ComponentConfigurationBuilder builder)
        => builder.AddDefaultResolvers().AddDefaultRenderers().AddDefaultInterceptors();

    /// <summary>
    /// 为 <see cref="ComponentConfigurationBuilder"/> 添加默认的解析器。
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultResolvers(this ComponentConfigurationBuilder builder)
        => builder.AddResolver<IParameterClassResolver, CssClassAttributeResolver>()
        .AddResolver<IHtmlAttributeResolver, HtmlAttributeAttributeResolver>()
        .AddResolver<IHtmlTagAttributeResolver, HtmlTagAttributeResolver>()
        ;

    /// <summary>
    /// 为 <see cref="ComponentConfigurationBuilder"/> 添加默认的拦截器。
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultInterceptors(this ComponentConfigurationBuilder builder)
        => builder.AddInterceptor<ChildContentInterceptor>()
        .AddInterceptor<AssociationComponentInterceptor>()
        .AddInterceptor<FormComponentInterceptor>()
        .AddInterceptor<CssClassAttributeInterceptor>()
        .AddInterceptor<StyleAttributeInterceptor>()
        ;

    /// <summary>
    /// 为 <see cref="ComponentConfigurationBuilder"/> 添加默认的渲染器。
    /// </summary>
    public static ComponentConfigurationBuilder AddDefaultRenderers(this ComponentConfigurationBuilder builder)
        => builder.AddRenderer<DefaultComponentRender>()
        .AddRenderer<NavLinkComponentRender>();
}
