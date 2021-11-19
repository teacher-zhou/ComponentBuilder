using System;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassResolver
    {
        public void Resolve(Type componentType);
    }
}
