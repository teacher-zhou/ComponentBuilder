using ComponentBuilder.Abstrations.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder;

/// <summary>
/// The extensions of DI for ComponentBuilder.
/// </summary>
public static class ComponentBuilderDependencyInjectionExtentions
{
    /// <summary>
    /// Add default serivces of ComponentBuilder to <see cref="IServiceCollection"/> instance.
    /// </summary>
    /// <param name="services">An instance of <see cref="IServiceCollection"/> to add.</param>
    public static IServiceCollection AddComponentBuilder(this IServiceCollection services) 
        => services.AddTransient<ICssClassBuilder, DefaultCssClassBuilder>()
            .AddTransient<IStyleBuilder, DefaultStyleBuilder>()
            .AddTransient<ICssClassAttributeResolver, CssClassAttributeResolver>()
            .AddTransient<IHtmlAttributesResolver, HtmlAttributeAttributeResolver>()
            .AddTransient<IHtmlAttributesResolver, HtmlDataAttributeResolver>()
            .AddTransient<IHtmlEventAttributeResolver, HtmlEventAttributeResolver>()
            .AddTransient<HtmlTagAttributeResolver>()
            ;
}
