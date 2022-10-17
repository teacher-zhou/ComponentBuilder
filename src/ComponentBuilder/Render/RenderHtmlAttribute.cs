using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Render
{
    public class RenderHtmlAttribute : OneOfBase<IDictionary<string, object>, object>
    {
        protected RenderHtmlAttribute(OneOf<IDictionary<string, object>, object> input) : base(input)
        {
        }

        public static implicit operator RenderHtmlAttribute(Dictionary<string, object> value)
            => new(OneOf<IDictionary<string, object>, object>.FromT0(value));
    }
}
