using OneOf;

namespace ComponentBuilder.Fluent;
/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> for <see cref="FluentRenderTreeBuilder"/>
/// </summary>
public static class FluentRenderTreeBuilderExtensions
{
    #region Element

    /// <summary>
    /// Represents an open element with specified name when condition is satisfied.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create element.</param>
    /// <param name="name">A value representing the type of the element.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public static IFluentAttributeBuilder Element(this RenderTreeBuilder builder, string name, string? @class = default, Condition? condition = default)
    {
        var render = new FluentRenderTreeBuilder(builder);
        if ( (condition is null || condition.Match(b => b, f => f())) )
        {
            return render.Element(name).Class(@class);
        }
        return render;
    }
    #endregion

    #region Component
    /// <summary>
    /// Represents an open component with specified component type.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="componentType">A type of component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component(this RenderTreeBuilder builder, Type componentType, Condition? condition=default)
    {
        var render = new FluentRenderTreeBuilder(builder);
        if(condition is null || condition.Match(b => b, f => f()) )
        {
            return render.Component(componentType);
        }
        return render;
    }
    /// <summary>
    /// Represents an open component with specified component.
    /// </summary>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component<TComponent>(this RenderTreeBuilder builder, Condition? condition = default)
        where TComponent : IComponent 
        => Component(builder, typeof(TComponent), condition);
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
    public static IFluentAttributeBuilder Attribute(this IFluentAttributeBuilder builder, string name, object? value, Condition? condition)
        => condition is null || condition.Match(b => b, f => f()) ? builder.Attribute(name, value) : builder;
    #endregion

    #region Class
    /// <summary>
    /// Add <c>class</c> HTML attribute to element.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="class">The CSS class string to add.</param>
    /// <param name="condition">A condition witch satisfied to add class attribute.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder Class(this IFluentAttributeBuilder builder, string? @class, Condition? condition = default)
        => @class.IsNotNullOrEmpty() ? builder.Attribute("class", $"{@class} ", condition) : builder;

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
    /// <param name="condition">A condition witch satisfied to add callback.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, string name, EventCallback callback, Condition? condition = default)
        => builder.Attribute(name, callback, condition);
    /// <summary>
    /// Add callback delegate to specify name of attribute or component.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add callback.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentAttributeBuilder Callback<TValue>(this IFluentAttributeBuilder builder, string name, EventCallback<TValue> callback, Condition? condition = default)
        => builder.Attribute(name, callback, condition);
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

    /// <summary>
    /// Create <c>&lt;div>...&lt;/div></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Div(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default)
        => builder.Element("div", @class, condition);

    /// <summary>
    /// Create <c>&lt;span>...&lt;/span></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Span(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default)
        => builder.Element("span", @class, condition);

    /// <summary>
    /// Create <c>&lt;a>...&lt;/a></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="href">The link of anchor.</param>
    /// <param name="target">The style to open link.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Anchor(this RenderTreeBuilder builder, string? href = default, string? @class = default, Condition? condition = default, string? target = "_blank")
        => builder.Element("a", @class, condition)
                    .Attribute("href", href, href.IsNotNullOrEmpty())
                    .Attribute("target", target, target.IsNotNullOrEmpty())
        ;

    /// <summary>
    /// Add <c>aria-{name}="{value}"</c> HTML attribute.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name">The name of aria.</param>
    /// <param name="value">The value of this attribute.</param>
    /// <param name="condition">A condition satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Aria(this IFluentAttributeBuilder builder, string name, object? value, Condition? condition=default)
        => builder.Attribute($"aria-{name}", value, condition);

    /// <summary>
    /// Add <c>data-{name}="{value}"</c> HTML attribute.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name">The name of data.</param>
    /// <param name="value">The value of this attribute.</param>
    /// <param name="condition">A condition satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Data(this IFluentAttributeBuilder builder, string name, object? value, Condition? condition=default)
        => builder.Attribute($"data-{name}", value, condition);
}
