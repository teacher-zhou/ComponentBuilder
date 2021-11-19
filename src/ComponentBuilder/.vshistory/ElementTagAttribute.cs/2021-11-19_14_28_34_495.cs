using System;

namespace ComponentBuilder
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class HtmlElementAttribute : Attribute
    {
        public HtmlElementAttribute(string elementName)
        {
            ElementName = elementName;
        }

        public string ElementName { get; }
    }
}
