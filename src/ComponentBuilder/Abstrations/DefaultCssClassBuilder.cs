using System;
using System.Collections.Generic;
using System.Linq;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Default implementation of <see cref="ICssClassBuilder"/> .
    /// </summary>
    public class DefaultCssClassBuilder : ICssClassBuilder
    {

        private readonly ICollection<string> _classes;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultCssClassBuilder"/> class.
        /// </summary>
        public DefaultCssClassBuilder() => _classes = new List<string>();

        /// <summary>
        /// Get all css class list.
        /// </summary>
        public IEnumerable<string> CssList => _classes;

        /// <summary>
        /// Append specified css class value.
        /// </summary>
        /// <param name="value">Css class value to append.</param>
        /// <returns>The instance of <see cref="ICssClassBuilder"/> .</returns>
        public ICssClassBuilder Append(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                _classes.Add(value);
            }

            return this;
        }

        /// <summary>
        /// Conbile all css class from list with space.
        /// </summary>
        public override string ToString() => string.Join(" ", _classes.Distinct());

        /// <summary>
        /// Clear css class string in container.
        /// </summary>
        public void Clear() => _classes.Clear();

        void IDisposable.Dispose() => Clear();
    }
}
