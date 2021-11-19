using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Bunit;
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
