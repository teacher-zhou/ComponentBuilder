
global using FluentAssertions;
global using Bunit;
global using System.Threading.Tasks;
global using Xunit;
global using System;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentBuilder.Test
{
    public abstract class ComponentBuilderTestBase
    {
        private readonly ServiceProvider _builder;

        protected ComponentBuilderTestBase()
        {
            var services = new ServiceCollection();
            services.AddComponentBuilder();
            _builder = services.BuildServiceProvider();
        }

        protected T GetService<T>() => _builder.GetService<T>();
    }
}
