using System;
using System.Linq;
using System.Reflection;

namespace ComponentBuilder.Abstrations
{
    public class DefaultCssClassAttributeResolver : ICssClassResolver, IDisposable
    {
        private readonly ICssClassBuilder _cssClassBuilder;
        private bool disposedValue;

        public DefaultCssClassAttributeResolver(ICssClassBuilder cssClassBuilder)
        {
            this._cssClassBuilder = cssClassBuilder;
        }

        public virtual string Resolve(object component)
        {
            if (component is null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            var componentType = component.GetType();
            if (componentType.TryGetAttribute<CssClassAttribute>(out var classCssAttribute))
            {
                _cssClassBuilder.Append(classCssAttribute.Css);
            }

            foreach (var parameters in componentType.GetProperties().Where(m => m.IsDefined(typeof(CssClassAttribute))))
            {
                var name = parameters.Name;
                var value = parameters.GetValue(component);

                var css = parameters.GetCustomAttribute<CssClassAttribute>().Css;



                if (value is null)
                {
                    continue;
                }

                switch (value)
                {
                    case Boolean:
                        if ((bool)value)
                        {
                            _cssClassBuilder.Append($"{css}");
                        }
                        break;
                    case Enum://css + enum css
                        _cssClassBuilder.Append($"{css}{((Enum)value).GetCssClass()}");
                        break;
                    default:// css + value
                        _cssClassBuilder.Append($"{css}{value}");
                        break;
                }
            }

            return _cssClassBuilder.Build(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    _cssClassBuilder.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~DefaultCssClassAttributeResolver()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
