using System;

namespace ComponentBuilder
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ElementTagAttribute : Attribute
    {
        public ElementTagAttribute(string name)
        {
            ElementName = name;
        }

        public string ElementName { get; }
    }
}
