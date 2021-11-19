using ComponentBuilder.Abstrations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
