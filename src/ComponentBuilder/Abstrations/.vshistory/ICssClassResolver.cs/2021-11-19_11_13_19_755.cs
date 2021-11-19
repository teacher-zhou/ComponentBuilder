using System;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassResolver
    {
        public string ResolveCssClass(Type componentType);
    }
}
