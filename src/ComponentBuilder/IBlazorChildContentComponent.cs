using ComponentBuilder.Abstrations;

namespace ComponentBuilder;

/// <summary>
/// Provides a blazor component has child content.
/// </summary>
public interface IBlazorChildContentComponent : IBlazorComponent, IHasChildContent
{
}

/// <summary>
/// Provides a blazor component has child content for a type of object.
/// </summary>
/// <typeparam name="TValue">A type of object.</typeparam>
public interface IBlazorChildContentComponent<TValue> : IBlazorComponent, IHasChildContent<TValue>
{

}