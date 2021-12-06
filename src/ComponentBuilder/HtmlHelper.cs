using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder
{
    public static class HtmlHelper
    {
        public static IEnumerable<KeyValuePair<string, object>> ResolveAttributes(object htmlAttributes)
        {
            if (htmlAttributes is null)
            {
                throw new ArgumentNullException(nameof(htmlAttributes));
            }

            if (htmlAttributes is IEnumerable<KeyValuePair<string, object>> keyValueAttributes)
            {
                return keyValueAttributes;
            }

            return htmlAttributes.GetType().GetProperties()
                .Select(property =>
                {
                    var name = property.Name.Replace("_", "-");
                    var value = property.GetValue(htmlAttributes);
                    return new KeyValuePair<string, object>(name, value);
                });
        }
    }
}
