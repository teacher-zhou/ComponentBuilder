namespace ComponentBuilder.Attributes
{
    /// <summary>
    /// Provides html element name for component to render.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class ElementTagAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ElementTagAttribute"/> class.
        /// </summary>
        /// <param name="name">Element tag name.</param>
        public ElementTagAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Gets the name of element tag.
        /// </summary>
        public string Name { get; }
    }
}
