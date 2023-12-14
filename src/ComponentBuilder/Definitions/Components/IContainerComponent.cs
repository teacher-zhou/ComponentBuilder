namespace ComponentBuilder.Definitions;

/// <summary>
/// Indicates that components that implement the interface are automatically added as containers. This interface is used for container components that are required by components during dynamic service invocation.
/// </summary>
public interface IContainerComponent:IComponent
{
}
