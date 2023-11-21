namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// A constructor that provides the content of an element.
/// </summary>
public interface IFluentContentBuilder :IFluentOpenBuilder, IFluentCloseBuilder
{
    /// <summary>
    /// Add fragment content to this element or component.
    /// </summary>
    /// <param name="fragment">The content fragment to insert the inner element.</param>
    IFluentContentBuilder Content(RenderFragment? fragment);
}

/// <summary>
/// Provides a constructor that adds subcontent for the specified component type.
/// </summary>
/// <typeparam name="TComponent">Component type.</typeparam>
public interface IFluentContentBuilder<TComponent> : IFluentContentBuilder,IFluentOpenComponentBuilder<TComponent> 
    where TComponent : IComponent
{
    /// <summary>
    /// Add an arbitrary code snippet to the component's <c>ChildContent</c> parameter.
    /// </summary>
    /// <param name="fragment">The content fragment to insert the inner element.</param>
    new IFluentAttributeBuilder<TComponent> Content(RenderFragment? fragment);
}