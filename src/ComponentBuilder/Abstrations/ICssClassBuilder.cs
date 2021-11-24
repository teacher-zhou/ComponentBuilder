using System;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Build a css class.
    /// </summary>
    public interface ICssClassBuilder : IDisposable
    {
        /// <summary>
        /// Append a value to builder.
        /// </summary>
        /// <param name="value">A value to append.</param>
        /// <returns>The instance of <see cref="ICssClassBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <c>null</c>.</exception>
        ICssClassBuilder Append(string value);

        /// <summary>
        /// Convert css class in container to string.
        /// </summary>
        /// <returns>A string separated by space for each item.</returns>
        string ToString();
    }
}
