using ComponentBuilder.Abstrations;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder
{
    /// <summary>
    /// The extensions for dependency injection.
    /// </summary>
    public static class ComponentBuilderDependencyInjectionExtentions
    {
        /// <summary>
        /// Add component builder default services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddComponentBuilder(this IServiceCollection services)
        {
            services.AddTransient<ICssClassBuilder, DefaultCssClassBuilder>()
                .AddTransient<ICssClassResolver, DefaultCssClassAttributeResolver>();
            return services;
        }
    }
}
