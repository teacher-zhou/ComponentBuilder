using System;

namespace ComponentBuilder
{
    /// <summary>
    /// Declare a value of css class to build.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class CssClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CssClassAttribute"/> class by given css class name.
        /// </summary>
        /// <param name="css"></param>
        public CssClassAttribute(string css)
        {
            Css = css;
        }

        public string Css { get; }
    }
}
