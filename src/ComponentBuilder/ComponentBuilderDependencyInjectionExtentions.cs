using ComponentBuilder.Abstrations;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder
{
    public static class ComponentBuilderDependencyInjectionExtentions
    {
        public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
        {
            services.AddTransient<ICssClassBuilder, DefaultCssClassBuilder>()
                .AddTransient<ICssClassResolver, DefaultCssClassAttributeResolver>();
            return services;
        }
    }
}
