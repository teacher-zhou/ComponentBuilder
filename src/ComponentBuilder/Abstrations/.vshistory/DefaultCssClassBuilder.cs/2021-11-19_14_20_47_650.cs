using System;
using System.Collections.Generic;
using System.Linq;

namespace ComponentBuilder.Abstrations
{
    public class DefaultCssClassBuilder : ICssClassBuilder
    {

        private readonly ICollection<string> _classes;
        private bool disposedValue;

        public DefaultCssClassBuilder() => _classes = new List<string>();

        public IEnumerable<string> CssList => _classes;

        public ICssClassBuilder Append(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }


            _classes.Add(value);
            return this;
        }

        public string Build() => string.Join(" ", _classes.Distinct());

        public void Dispose() => _classes.Clear();
    }
}
