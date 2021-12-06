using System;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Resolve attributes from parameters in component.
    /// </summary>
    /// <typeparam name="TResult">The result type after resolved.</typeparam>
    public interface IComponentParameterResolver<TResult>
    {
        /// <summary>
        /// Resolve css class from specified component.
        /// </summary>
        /// <param name="component">The parameters in component to resolve.</param>
        /// <returns>A css class string separated by spece for each item.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="component"/> is null.</exception>
        public TResult Resolve(BlazorComponentBase component);
    }
}
