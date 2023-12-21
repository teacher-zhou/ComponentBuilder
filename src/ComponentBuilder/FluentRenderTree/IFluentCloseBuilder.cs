namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// Provides a constructor that closes the flag.
/// </summary>
public interface IFluentCloseBuilder : IDisposable
{
    /// <summary>
    /// Mark a previously attached region, element, or component as closed. 
    /// Call this method when must with previous <c> Element()</c>, <c> Component() </c> or <c> Region() </c> match.
    /// </summary>
    IFluentOpenBuilder Close();
}

/// <summary>
/// Provides a constructor to close component tags.
/// </summary>
/// <typeparam name="TComponent">Component type.</typeparam>
public interface IFluentCloseBuilder<TComponent> : IFluentCloseBuilder, IDisposable where TComponent : IComponent
{
    /// <summary>
    /// Mark a previously attached region, or component as closed. 
    /// Call this method when must with previous <c> Component() </c> or <c> Region() </c> match.
    /// </summary>
    new IFluentOpenComponentBuilder<TComponent> Close();
}