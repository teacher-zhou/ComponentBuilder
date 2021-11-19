using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test
{
    internal class ComponentBuilderTestBase
    {
        private readonly ServiceProvider _builder;

        public ComponentBuilderTestBase()
        {
            var services = new ServiceCollection();
            services.AddComponentBuilder();
            _builder = services.BuildServiceProvider();
        }

        protected T GetService<T>() => _builder.GetService<T>();
    }
}
