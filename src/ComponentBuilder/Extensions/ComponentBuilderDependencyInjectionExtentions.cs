using ComponentBuilder.Abstrations.Internal;

using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder;

/// <summary>
/// 扩展服务。
/// </summary>
public static class ComponentBuilderDependencyInjectionExtentions
{
    /// <summary>
    /// 添加 ComponentBuilder 的默认服务到 <see cref="IServiceCollection"/> 实例。
    /// </summary>
    /// <param name="services">要添加服务的 <see cref="IServiceCollection"/> 实例。</param>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
    {
        services.AddTransient<ICssClassBuilder, DefaultCssClassBuilder>()
            .AddTransient<IStyleBuilder, DefaultStyleBuilder>()
            .AddTransient<ICssClassAttributeResolver, CssClassAttributeResolver>()
            .AddTransient<IHtmlAttributesResolver, HtmlAttributeAttributeResolver>()
            .AddTransient<IHtmlAttributesResolver, HtmlDataAttributeResolver>()
            .AddTransient<IHtmlEventAttributeResolver, HtmlEventAttributeResolver>()
            .AddTransient<HtmlTagAttributeResolver>()
            ;
        return services;
    }

    public static IServiceCollection RegisterComponent(this IServiceCollection services, Type componentServiceType, Type componentRenderType) => services.AddScoped(componentServiceType, componentRenderType);
    public static IServiceCollection RegisterComponent<TComponent, TComponentRender>(this IServiceCollection services)
        => services.RegisterComponent(typeof(TComponent), typeof(TComponentRender));
}
