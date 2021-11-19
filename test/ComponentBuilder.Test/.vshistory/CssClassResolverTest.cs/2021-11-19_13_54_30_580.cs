using ComponentBuilder.Abstrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Test
{
    public class CssClassResolverTest : ComponentBuilderTestBase
    {
        private readonly ICssClassResolver _resolver;
        public CssClassResolverTest()
        {
            _resolver = GetService<ICssClassResolver>();
        }
    }
}
