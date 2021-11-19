using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentBuilder.Abstrations
{
    public class DefaultCssClassBuilder : ICssClassBuilder
    {

        private readonly ICollection<string> _classes;
        public DefaultCssClassBuilder()
        {
            _classes = new List<string>();
        }

        public IEnumerable<string> CssList => _classes;

        public ICssClassBuilder Append(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
            }

            _classes.Add(name);
            return this;
        }

        public string Build() => string.Join(" ", _classes);

        public void Dispose()
        {
            _classes.Clear();
        }
    }
}
