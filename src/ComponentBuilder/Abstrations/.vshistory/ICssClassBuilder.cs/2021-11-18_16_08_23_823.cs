using System;

namespace ComponentBuilder.Abstrations
{
    public interface ICssClassBuilder : IDisposable
    {
        ICssClassBuilder Append(string name);
        string Build();
    }
}
