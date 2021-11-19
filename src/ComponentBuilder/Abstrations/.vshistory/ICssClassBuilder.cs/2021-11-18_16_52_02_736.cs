using System;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Build a css class.
    /// </summary>
    public interface ICssClassBuilder : IDisposable
    {
        /// <summary>
        /// Append a value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ICssClassBuilder Append(string value);

        string Build();
    }
}
