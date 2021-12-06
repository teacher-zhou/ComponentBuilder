using System;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Provides a resolver to resolve attribute from component.
    /// </summary>
    public interface IComponentParameterResolver<TResult> where TResult : class
    {
        /// <summary>
        /// Resolve css class from specified component.
        /// </summary>
        /// <param name="component">The component to resolve.</param>
        /// <returns>A css class string separated by spece for each item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        public TResult Resolve(BlazorComponentBase component);
    }
}
