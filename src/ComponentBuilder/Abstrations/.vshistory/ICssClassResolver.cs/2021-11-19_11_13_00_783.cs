using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassResolver
    {
        public string ResolveCssClassAttributes(Type componentType);
    }
}
