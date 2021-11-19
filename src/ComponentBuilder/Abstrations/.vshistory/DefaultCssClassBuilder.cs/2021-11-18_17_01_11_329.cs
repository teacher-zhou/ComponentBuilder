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

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _classes.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~DefaultCssClassBuilder()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
