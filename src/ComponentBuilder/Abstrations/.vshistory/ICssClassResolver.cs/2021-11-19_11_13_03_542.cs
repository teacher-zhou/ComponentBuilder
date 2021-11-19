using System;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassResolver
    {
        public string ResolveCssClassAttributes(Type componentType);
    }
}
