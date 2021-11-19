using System;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Provides a resolver to resolve css class for component.
    /// </summary>
    public interface ICssClassResolver
    {
        /// <summary>
        /// Resolve css class from specified component.
        /// </summary>
        /// <param name="component">The component to resolve.</param>
        /// <returns>A css class string separated by spece for each item.</returns>
        public string Resolve(object component);
    }
}
