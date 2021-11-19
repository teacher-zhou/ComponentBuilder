using ComponentBuilder.Abstrations;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder
{
    public static class ComponentBuilderDependencyInjectionExtentions
    {
        public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
        {
            services.AddScoped<ICssClassBuilder, DefaultCssClassBuilder>()
                .AddScoped<ICssClassResolver, DefaultCssClassAttributeResolver>();
            return services;
        }
    }
}
