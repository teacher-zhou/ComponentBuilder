using System;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassResolver
    {
        public void ResolveCssClass(Type componentType);
    }
}
