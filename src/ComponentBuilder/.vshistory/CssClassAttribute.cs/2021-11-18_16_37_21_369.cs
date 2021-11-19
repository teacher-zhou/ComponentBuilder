using System;

namespace ComponentBuilder
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CssClassAttribute : Attribute
    {
        public CssClassAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
