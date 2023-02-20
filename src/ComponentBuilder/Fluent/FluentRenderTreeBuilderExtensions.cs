

namespace ComponentBuilder.Fluent;
/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> for <see cref="FluentRenderTreeBuilder"/>
/// </summary>
public static class FluentRenderTreeBuilderExtensions
{
    #region Element
    /// <summary>
    /// Represents an open element with specified name.
    /// </summary>
    /// <param name="name">A value representing the type of the element.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code. 
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public static IFluentAttributeBuilder Element(this RenderTreeBuilder builder, string name, int? sequence = default)
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return render.Element(name);
    }

    /// <summary>
    /// Represents an open element with specified name when condition is satisfied.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create element.</param>
    /// <param name="name">A value representing the type of the element.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code. 
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public static IFluentAttributeBuilder ElementIf(this RenderTreeBuilder builder, bool condition, string name, int? sequence = default)
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return condition ? builder.Element(name, sequence) : render;
    }
    #endregion

    #region Component
    /// <summary>
    /// Represents an open component with specified component type.
    /// </summary>
    /// <param name="componentType">A type of component.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component(this RenderTreeBuilder builder, Type componentType, int? sequence = default)
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return render.Component(componentType);
    }
    /// <summary>
    /// Represents an open component with specified component type when condition is satisfied.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="componentType">A type of component.</param>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder ComponentIf(this RenderTreeBuilder builder, bool condition, Type componentType, int? sequence = default)
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return condition ? builder.Component(componentType, sequence) : render;
    }

    /// <summary>
    /// Represents an open component with specified component.
    /// </summary>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component<TComponent>(this RenderTreeBuilder builder, int? sequence = default)
        where TComponent : IComponent
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return render.Component(typeof(TComponent));
    }

    /// <summary>
    /// Represents an open component with specified component when condition is satisfied.
    /// </summary>
    /// <param name="sequence">An integer that represents the start position of the instruction in the source code.
    /// <para>
    /// <c>null</c> to create by framework.
    /// </para>
    /// </param>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component<TComponent>(this RenderTreeBuilder builder, bool condition, int? sequence = default)
        where TComponent : IComponent
    {
        var render = new FluentRenderTreeBuilder(builder, sequence);
        return condition ? builder.Component<TComponent>(sequence) : render;
    }
    #endregion

    #region Content
    /// <summary>
    /// Add text string to this element or component. Multiple content will be combined for multiple invocation.    
    /// </summary>
    /// <param name="text">The text string to insert into inner element.</param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <returns>A <see cref="IFluentContentBuilder"/> instance contains inner content.</returns>
    public static IFluentContentBuilder Content(this IFluentContentBuilder builder, string? text)
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// Add inner markup string to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="markup">The markup content to insert into inner element.</param>
    /// <returns>A <see cref="IFluentContentBuilder"/> instance contains inner content.</returns>
    public static IFluentContentBuilder Content(this IFluentContentBuilder builder, MarkupString markup)
        => builder.Content(b => b.AddContent(0, markup));

    /// <summary>
    /// Add a fragment with specified value to inner component. 
    /// Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <typeparam name="TValue">The type of value.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentContentBuilder"/>.</param>
    /// <param name="fragment">A fragment content.</param>
    /// <param name="value">The value of fragment context.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains inner content.</returns>
    public static IFluentContentBuilder Content<TValue>(this IFluentContentBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Content(b => b.AddContent(0, fragment, value));
    #endregion

    #region Attribute
    /// <summary>
    /// Add element attribute or component parameter and attribute when condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentContentBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="IFluentContentBuilder"/> instance contains attrbutes or parameters.</returns>
    public static IFluentAttributeBuilder AttributeIf(this IFluentAttributeBuilder builder, bool condition, string name, object? value)
        => condition ? builder.Attribute(name, value) : builder;
    #endregion

    #region Class
    /// <summary>
    /// Add <c>class</c> HTML attribute to element.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="classes">The CSS class string to add.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder Class(this IFluentAttributeBuilder builder, params string[] classes)
    {
        if (classes is not null)
        {
            foreach (var item in classes)
            {
                builder.Attribute("class", $"{item} ");
            }
        }
        return builder;
    }

    /// <summary>
    /// Add <c>class</c> HTML attribute to element if condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add class attribute.</param>
    /// <param name="classes">The CSS class string to add.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder ClassIf(this IFluentAttributeBuilder builder, bool condition, params string[] classes)
       => condition ? builder.Class(classes) : builder;

    #endregion

    #region Style
    /// <summary>
    /// Add <c>style</c> HTML attribute to element.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="styles">The style string to add.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder Style(this IFluentAttributeBuilder builder, params string?[] styles)
    {
        if (styles is not null)
        {
            foreach (var item in styles)
            {
                builder.Attribute("style", $"{item}; ");
            }
        }
        return builder;
    }

    /// <summary>
    /// Add <c>style</c> HTML attribute to element.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="styles">The style string to add.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder Style(this IFluentAttributeBuilder builder, params (string name, object? value)[] styles)
    {
        if (styles is not null)
        {
            foreach (var (name, value) in styles)
            {
                builder.Attribute("style", $"{name}:{value}; ");
            }
        }
        return builder;
    }

    /// <summary>
    /// Add <c>style</c> HTML attribute to element if condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add style attribute.</param>
    /// <param name="styles">The style string to add.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder StyleIf(this IFluentAttributeBuilder builder, bool condition, params string?[] styles)
       => condition ? builder.Style(styles) : builder;


    /// <summary>
    /// Add <c>style</c> HTML attribute to element if condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add style attribute.</param>
    /// <param name="styles">The style string to add.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder StyleIf(this IFluentAttributeBuilder builder, bool condition, params (string name, object? value)[] styles)
       => condition ? builder.Style(styles) : builder;
    #endregion

    #region Callback
    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, string name, EventCallback callback)
        => builder.Attribute(name, callback);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    public static IFluentAttributeBuilder Callback<TValue>(this IFluentAttributeBuilder builder, string name, EventCallback<TValue> callback)
        => builder.Attribute(name, callback);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component when condition is satisfied.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add callback attribute.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder CallbackIf(this IFluentAttributeBuilder builder, bool condition, string name, EventCallback callback)
        => builder.AttributeIf(condition, name, callback);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component when condition is satisfied.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add callback attribute.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains event attribute.</returns>
    public static IFluentAttributeBuilder CallbackIf<TValue>(this IFluentAttributeBuilder builder, bool condition, string name, EventCallback<TValue> callback)
        => builder.AttributeIf(condition, name, callback);
    #endregion

    #region Ref
    /// <summary>
    /// Capture the reference of component after rendered.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="captureReferenceAction">An action to capture the reference of element after component is rendered.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance that reference is captured.</returns>
    public static IFluentAttributeBuilder Ref<TComponent>(this IFluentAttributeBuilder builder, Action<TComponent?> captureReferenceAction) where TComponent : IComponent
    {
        builder.Ref(el => captureReferenceAction((TComponent?)el));
        return builder;
    }

    /// <summary>
    /// Captures the reference for element after rendered.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="captureReferenceAction">An action to capture the reference of element after component is rendered.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance that reference is captured.</returns>
    public static IFluentAttributeBuilder Ref(this IFluentAttributeBuilder builder, Action<ElementReference?> captureReferenceAction)
    {
        builder.Ref(el => captureReferenceAction((ElementReference?)el));
        return builder;
    }
    #endregion

    #region MultipleAttributes
    /// <summary>
    /// Add multiple attributes to element or component.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="attributes">The attributes array witch contains HTML attributes or parameters.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance that contains attributes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="attributes"/> is null.</exception>
    public static IFluentAttributeBuilder MultipleAttributes(this IFluentAttributeBuilder builder, IEnumerable<KeyValuePair<string, object>> attributes)
    {
        if ( attributes is null )
        {
            throw new ArgumentNullException(nameof(attributes));
        }
        attributes.ForEach(item => builder.Attribute(item.Key, item.Value));

        return builder;
    }

    /// <summary>
    /// Add multiple attributes to element or component with anonymouse object.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="attributes">
    /// The attributes object witch contains HTML attributes or parameters.
    /// <para>Example:
    /// <code language="cs">
    /// new { id = "#id", @class = "my-class", Actived = true, ...}
    /// </code>
    /// NOTE: Use `@` for keyword like `@class`; Use '_' instead of '-' like 'data_toggle'
    /// </para>
    /// </param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance that contains attributes.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="attributes"/> is null.</exception>
    public static IFluentAttributeBuilder MultipleAttributes(this IFluentAttributeBuilder builder, object? attributes)
    {
        if ( attributes is null )
        {
            throw new ArgumentNullException(nameof(attributes));
        }

        builder.MultipleAttributes(HtmlHelper.MergeHtmlAttributes(attributes)!);

        return builder;
    }
    #endregion
}
