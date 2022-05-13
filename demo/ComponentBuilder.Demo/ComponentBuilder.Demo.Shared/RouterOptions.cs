using System.Reflection;

namespace ComponentBuilder.Components.Demo.Shared;
public class RouterOptions
{
    public Assembly AppAssembly { get; set; }

    public IEnumerable<Assembly> AdditionalAssemblies { get; set; }
}
