using ComponentBuilder;

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
        var render = new BlazorRenderTree(builder,sequence);
        return render.Open<TComponent>();
    }
}
