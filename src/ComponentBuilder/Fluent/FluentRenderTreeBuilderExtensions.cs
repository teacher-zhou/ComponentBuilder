

namespace ComponentBuilder.Fluent;
/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> for <see cref="FluentRenderTreeBuilder"/>
/// </summary>
public static class FluentRenderTreeBuilderExtensions
{
    /// <summary>
    /// Represents an open element with specified name.
    /// <para>
    /// You have to also call <see cref="FluentRenderTreeBuilder.Close"/> after element finish building or you can use <c>using</c> scoped-block like this:
    /// <code language="cs">
    /// using var render = builder.Open("div");
    /// </code>
    /// or
    /// <code language="cs">
    /// using(var render = bulder.Open("div"))
    /// {
    ///     //...
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="elementName">A value representing the type of the element.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code. 
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public static FluentRenderTreeBuilder Element(this RenderTreeBuilder builder, string elementName, int? sequence = default)
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return render.Element(elementName);
    }

    /// <summary>
    /// Represents an open component with specified component type.
    /// <para>
    /// You have to also call <see cref="FluentRenderTreeBuilder.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
    /// <code language="cs">
    /// using var render = builder.Open(typeof(MyCompenent));
    /// </code>
    /// or
    /// <code language="cs">
    /// using(var render = bulder.Open(typeof(MyCompenent)))
    /// {
    ///     //...
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="componentType">A type of component.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static FluentRenderTreeBuilder Component(this RenderTreeBuilder builder, Type componentType, int? sequence = default)
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return render.Component(componentType);
    }

    /// <summary>
    /// Represents an open component with specified component.
    /// <para>
    /// You have to also call <see cref="FluentRenderTreeBuilder.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
    /// <code language="cs">
    /// using var render = builder.Open(typeof(MyCompenent));
    /// </code>
    /// or
    /// <code language="cs">
    /// using(var render = bulder.Open(typeof(MyCompenent)))
    /// {
    ///     //...
    /// }
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static FluentRenderTreeBuilder Component<TComponent>(this RenderTreeBuilder builder, int? sequence = default) where TComponent : IComponent
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return render.Component<TComponent>();
    }
    #region Content
    /// <summary>
    /// Add text string to this element or component. Multiple content will be combined for multiple invocation.    
    /// </summary>
    /// <param name="text">The text string to insert into inner element.</param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    public static FluentRenderTreeBuilder Content(this FluentRenderTreeBuilder builder, string? text) 
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// Add inner markup string to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="markup">The markup content to insert into inner element.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    public static FluentRenderTreeBuilder Content(this FluentRenderTreeBuilder builder, MarkupString markup)
        => builder.Content(b => b.AddContent(0, markup));
    #endregion

    /// <summary>
    /// Add element attribute or component parameter and attribute when condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains attrbutes or parameters.</returns>
    public static FluentRenderTreeBuilder AttributeIf(this FluentRenderTreeBuilder builder, bool condition, string name, object? value)
        => condition ? builder.Attribute(name, value) : builder;

    #region Class
    /// <summary>
    /// Add <c>class</c> HTML attribute to element.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="cssClass">The CSS class string to add.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static FluentRenderTreeBuilder Class(this FluentRenderTreeBuilder builder, string? cssClass)
        => builder.Attribute("class", cssClass);

    /// <summary>
    /// Add <c>class</c> HTML attribute to element if condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add class attribute.</param>
    /// <param name="cssClass">The CSS class string to add.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static FluentRenderTreeBuilder ClassIf(this FluentRenderTreeBuilder builder, bool condition, string? cssClass)
       => condition ? builder.Attribute("class", cssClass) : builder;
    #endregion

    #region Callback
    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// <para>
    /// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    /// </para>
    /// <para>
    /// Example:
    /// <code language="cs">
    /// Htmlhelper.Event.Create(this, () => { 
    ///     // you callback code here...
    /// })
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static FluentRenderTreeBuilder Callback(this FluentRenderTreeBuilder builder, string name, EventCallback callback)
        => builder.Attribute(name, callback);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// <para>
    /// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    /// </para>
    /// <para>
    /// Example:
    /// <code language="cs">
    /// Htmlhelper.CreateCallback&lt;TEventArgs>(this, (e) => { 
    ///     // you callback code here...
    /// })
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    public static FluentRenderTreeBuilder Callback<TValue>(this FluentRenderTreeBuilder builder, string name, EventCallback<TValue> callback)
        => builder.Attribute(name, callback);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component when condition is satisfied.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// <para>
    /// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    /// </para>
    /// <para>
    /// Example:
    /// <code language="cs">
    /// Htmlhelper.Event.Create(this, () => { 
    ///     // you callback code here...
    /// })
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add callback attribute.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static FluentRenderTreeBuilder CallbackIf(this FluentRenderTreeBuilder builder, bool condition, string name, EventCallback callback)
        => builder.AttributeIf(condition, name, callback);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component when condition is satisfied.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// <para>
    /// Recommend to use <see cref="HtmlHelper"/> <see langword="static"/> class to create the callback.
    /// </para>
    /// <para>
    /// Example:
    /// <code language="cs">
    /// Htmlhelper.CreateCallback&lt;TEventArgs>(this, (e) => { 
    ///     // you callback code here...
    /// })
    /// </code>
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add callback attribute.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    public static FluentRenderTreeBuilder CallbackIf<TValue>(this FluentRenderTreeBuilder builder, bool condition, string name, EventCallback<TValue> callback)
        => builder.AttributeIf(condition, name, callback);
    #endregion
}
