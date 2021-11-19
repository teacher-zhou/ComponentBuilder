using System;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassResolver
    {
        public string Resolve(object component);
    }
}
