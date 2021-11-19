using System;

namespace ComponentBuilder.Abstrations
{
    /// <summary>
    /// Build a css class.
    /// </summary>
    public interface ICssClassBuilder : IDisposable
    {
        ICssClassBuilder Append(string name);

        string Build();
    }
}
