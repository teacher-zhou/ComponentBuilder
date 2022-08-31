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

    /// <summary>
    /// 注册指定类型的组件作为服务和实现。只有组件定义了特性 <see cref="ServiceComponentAttribute"/> 后需要注册。
    /// </summary>
    /// <param name="services">要添加服务的 <see cref="IServiceCollection"/> 实例。</param>>
    /// <param name="componentServiceType">组件服务类型。该组件需要定义特性 <see cref="ServiceComponentAttribute"/> 。</param>
    /// <param name="componentImplementationType">组件实现类型。</param>
    /// <exception cref="InvalidOperationException">服务组件没有定义 <see cref="ServiceComponentAttribute"/> 特性。</exception>
    public static IServiceCollection RegisterComponent(this IServiceCollection services, Type componentServiceType, Type componentImplementationType)
    {
        if (!componentServiceType.IsDefined(typeof(ServiceComponentAttribute), false))
        {
            throw new InvalidOperationException($"组件 '{componentServiceType.Name}' 必须定义 '{nameof(ServiceComponentAttribute)}' 特性才可以注册为服务");
        }

        services.AddScoped(componentServiceType, componentImplementationType);
        return services;
    }
    /// <summary>
    /// 注册指定类型的组件作为服务和实现。只有组件定义了特性 <see cref="ServiceComponentAttribute"/> 后需要注册。
    /// </summary>
    /// <typeparam name="TComponentService">服务组件类型。</typeparam>
    /// <typeparam name="TComponentImplementation">实现组件类型。</typeparam>
    /// <param name="services">要添加服务的 <see cref="IServiceCollection"/> 实例。</param>
    public static IServiceCollection RegisterComponent<TComponentService, TComponentImplementation>(this IServiceCollection services)
        where TComponentService : ComponentBase
        where TComponentImplementation : TComponentService
        => services.RegisterComponent(typeof(TComponentService), typeof(TComponentImplementation));
}
