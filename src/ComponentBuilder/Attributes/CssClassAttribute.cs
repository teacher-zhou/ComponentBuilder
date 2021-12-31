namespace ComponentBuilder
{
    /// <summary>
    /// Declare a value of CSS class to build.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
    public class CssClassAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CssClassAttribute" /> class.
        /// </summary>
        public CssClassAttribute() : this(string.Empty)
        {

        }

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
        /// <summary>
        /// Gets or sets order from small to large to create CSS class.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Disable to apply CSS value. 
        /// <para>
        /// This property is useless in interface definition.
        /// </para>
        /// </summary>
        public bool Disabled { get; set; }
    }
}
