namespace ComponentBuilder;
/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> for <see cref="BlazorRenderTree"/>
/// </summary>
public static class BlazorRenderTreeExtensions
{
    /// <summary>
    /// Represents an open element with specified name.
    /// <para>
    /// You have to also call <see cref="BlazorRenderTree.Close"/> after element finish building or you can use <c>using</c> scoped-block like this:
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
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open element.</returns>
    public static BlazorRenderTree Open(this RenderTreeBuilder builder, string elementName, int? sequence = default)
    {
        var render = new BlazorRenderTree(builder, sequence);
        return render.Open(elementName);
    }

    /// <summary>
    /// Represents an open component with specified component type.
    /// <para>
    /// You have to also call <see cref="BlazorRenderTree.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
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
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open component.</returns>
    public static BlazorRenderTree Open(this RenderTreeBuilder builder, Type componentType, int? sequence = default)
    {
        var render = new BlazorRenderTree(builder, sequence);
        return render.Open(componentType);
    }

    /// <summary>
    /// Represents an open component with specified component.
    /// <para>
    /// You have to also call <see cref="BlazorRenderTree.Close"/> after component finish building or you can use <c>using</c> scoped-block instead:
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
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open component.</returns>
    public static BlazorRenderTree Open<TComponent>(this RenderTreeBuilder builder, int? sequence = default) where TComponent : IComponent
    {
        var render = new BlazorRenderTree(builder, sequence);
        return render.Open<TComponent>();
    }

    /// <summary>
    /// Represents an open element with name of div.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code. </param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open element.</returns>
    public static BlazorRenderTree Div(this RenderTreeBuilder builder, int? sequence = default)
        => builder.Open("div", sequence);

    /// <summary>
    /// Represents an open element with name of span.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code. </param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains an open element.</returns>
    public static BlazorRenderTree Span(this RenderTreeBuilder builder, int? sequence = default)
        => builder.Open("span", sequence);

    /// <summary>
    /// Add element attribute or component parameter and attribute when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="tree">The instance <see cref="BlazorRenderTree"/> class.</param>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <param name="condition">The condition satisified to add attribute.</param>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains attrbutes or parameters.</returns>
    public static BlazorRenderTree Attributes<TValue>(this BlazorRenderTree tree, string name, TValue? value, bool condition)
    {
        if (!condition)
        {
            return tree;
        }

        return tree.Attributes(name, value);
    }

    /// <summary>
    /// Add callback delegate to specify name of attribute or component when <paramref name="condition"/> is <c>true</c>.
    /// </summary>
    /// <param name="tree">The instance <see cref="BlazorRenderTree"/> class.</param>
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
    /// <param name="condition">The condition satisified to add callback.</param>
    /// <returns>A <see cref="BlazorRenderTree"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static BlazorRenderTree EventCallback(this BlazorRenderTree tree, string name, EventCallback callback, bool condition)
    {
        if (!condition)
        {
            return tree;
        }
        return tree.EventCallback(name, callback);
    }
}
