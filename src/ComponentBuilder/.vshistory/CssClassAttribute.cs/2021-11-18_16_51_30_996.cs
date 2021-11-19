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
        /// <param name="css">The css class value.</param>
        public CssClassAttribute(string css)
        {
            Css = css ?? throw new ArgumentNullException(nameof(css));
        }

        /// <summary>
        /// Gets css class value.
        /// </summary>
        public string Css { get; }
    }
}
