
global using FluentAssertions;
global using Bunit;
global using System.Threading.Tasks;
global using Xunit;
global using System;
global using FluentAssertions.BUnit;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComponentBuilder.Test
{
    public abstract class TestBase
    {
        private readonly ServiceProvider _builder;

        protected TestBase()
        {
            var services = new ServiceCollection();

            services.AddComponentBuilder();
            _builder = services.BuildServiceProvider();

            TestContext.Services.AddComponentBuilder();
        }

        protected T GetService<T>() => _builder.GetService<T>();

        protected TestContext TestContext { get; set; } = new TestContext();
    }
}
