namespace ComponentBuilder.Fluent;
/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> for <see cref="FluentRenderTreeBuilder"/>
/// </summary>
public static class FluentRenderTreeBuilderExtensions
{
    /// <summary>
    /// Create render tree withing fluent API.
    /// </summary>
    /// <param name="builder">The instance <see cref="RenderTreeBuilder"/> class.</param>
    /// <returns></returns>
    public static IFluentOpenBuilder Fluent(this RenderTreeBuilder builder) => new FluentRenderTreeBuilder(builder);

    #region Element

    /// <summary>
    /// Represents an open element with specified name when condition is satisfied.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create element.</param>
    /// <param name="name">A value representing the type of the element.</param>
    /// <param name="class"></param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public static IFluentAttributeBuilder Element(this IFluentOpenElementBuilder builder, string name, string? @class = default, Condition? condition = default, int? sequence = default)
        => Condition.Execute(condition, () => builder.Element(name, sequence).Class(@class), () => (FluentRenderTreeBuilder)builder);

    /// <summary>
    /// Represents an open element with specified name when condition is satisfied.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create element.</param>
    /// <param name="name">A value representing the type of the element.</param>
    /// <param name="class"></param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open element.</returns>
    public static IFluentAttributeBuilder Element(this RenderTreeBuilder builder, string name, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Element(name, @class, condition, sequence);
    #endregion

    #region Component
    /// <summary>
    /// Represents an open component with specified component type.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="componentType">A type of component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component(this IFluentOpenComponentBuilder builder, Type componentType, Condition? condition = default, int? sequence = default)
        => Condition.Execute(condition, () => builder.Component(componentType, sequence), () => (FluentRenderTreeBuilder)builder);

    /// <summary>
    /// Represents an open component with specified component type.
    /// </summary>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="componentType">A type of component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component(this RenderTreeBuilder builder, Type componentType, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Component(componentType, condition, sequence);

    /// <summary>
    /// Represents an open component with specified component.
    /// </summary>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component<TComponent>(this IFluentOpenComponentBuilder builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
        => builder.Component(typeof(TComponent), condition, sequence);

    /// <summary>
    /// Represents an open component with specified component.
    /// </summary>
    /// <typeparam name="TComponent">A type of component.</typeparam>
    /// <param name="condition">A condition that satisfied to create component.</param>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/></param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains an open component.</returns>
    public static IFluentAttributeBuilder Component<TComponent>(this RenderTreeBuilder builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
        => builder.Fluent().Component<TComponent>(condition, sequence);

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

    /// <summary>
    /// Add text string to this element or component. Multiple content will be combined for multiple invocation.    
    /// </summary>
    /// <param name="text">The text string to insert into inner element.</param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <returns>A <see cref="IFluentContentBuilder"/> instance contains inner content.</returns>
    public static IFluentOpenBuilder Content(this IFluentOpenBuilder builder, string? text)
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// Add inner markup string to this element or component. Multiple content will be combined for multiple invocation.
    /// </summary>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="markup">The markup content to insert into inner element.</param>
    /// <returns>A <see cref="IFluentContentBuilder"/> instance contains inner content.</returns>
    public static IFluentOpenBuilder Content(this IFluentOpenBuilder builder, MarkupString markup)
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
    public static IFluentOpenBuilder Content<TValue>(this IFluentOpenBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Content(b => b.AddContent(0, fragment, value));
    #endregion

    #region Attribute
    /// <summary>
    /// Add element attribute or component parameter and attribute when condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <param name="name">The name of HTML attribute or parameter.</param>
    /// <param name="value">The value of attribute or parameter.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    public static IFluentAttributeBuilder Attribute(this IFluentAttributeBuilder builder, string name, object? value, Condition? condition)
        => Condition.Execute(condition, () => builder.Attribute(name, value), () => builder);

    /// <summary>
    /// Add element attribute or component parameter and attribute when condition is satisfied.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <param name="attributes">The key/value paires of attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    public static IFluentAttributeBuilder Attribute(this IFluentAttributeBuilder builder, object attributes, Condition? condition = default)
    {
        HtmlHelper.MergeHtmlAttributes(attributes)?.ForEach(item => builder.Attribute(item.Key, item.Value, condition));
        return builder;
    }

    /// <summary>
    /// Add fragment content to ChildContent attribute of component.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="fragment">A fragment content.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    public static IFluentAttributeBuilder ChildContent(this IFluentAttributeBuilder builder, RenderFragment? fragment, Condition? condition = default)
        => builder.Attribute("ChildContent", fragment, condition);

    /// <summary>
    /// Add fragment content to ChildContent attribute of component.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="markup">The markup content to insert into inner element.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    public static IFluentAttributeBuilder ChildContent(this IFluentAttributeBuilder builder, MarkupString? markup, Condition? condition = default)
        => builder.Attribute("ChildContent", markup, condition);

    /// <summary>
    /// Add fragment content to ChildContent attribute of component.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentAttributeBuilder"/>.</param>
    /// <param name="text">The text string to insert into inner element.</param>
    /// <param name="condition">A condition witch satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains attrbutes or parameters.</returns>
    public static IFluentAttributeBuilder ChildContent(this IFluentAttributeBuilder builder, string? text, Condition? condition = default)
        => builder.Attribute("ChildContent", text, condition);
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
    /// <param name="style">The style string to add.</param>
    /// <param name="condition">A condition witch satisfied to add style attribute.</param>
    /// <returns>A <see cref="FluentRenderTreeBuilder"/> instance contains class attribute.</returns>
    public static IFluentAttributeBuilder Style(this IFluentAttributeBuilder builder, string? style, Condition? condition = default)
        => builder.Attribute("style", style, condition);
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
    /// Add callback delegate to specify name of callback action.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="receiver">The receiver of event.</param>
    /// <param name="condition">A condition witch satisfied to add callback.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, string name, object receiver, Action callback, Condition? condition = default)
     => builder.Callback(name, HtmlHelper.Event.Create(receiver, callback), condition);

    /// <summary>
    /// Add callback delegate to specify name of callback function.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="receiver">The receiver of event.</param>
    /// <param name="condition">A condition witch satisfied to add callback.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, string name, object receiver, Func<Task> callback, Condition? condition = default)
     => builder.Callback(name, HtmlHelper.Event.Create(receiver, callback), condition);

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

    /// <summary>
    /// Add callback delegate to specify name of callback action.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="receiver">The receiver of event.</param>
    /// <param name="condition">A condition witch satisfied to add callback.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentAttributeBuilder Callback<TValue>(this IFluentAttributeBuilder builder, string name, object receiver, Action<TValue> callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Event.Create(receiver, callback), condition);

    /// <summary>
    /// Add callback delegate to specify name of callback function.
    /// </summary>
    /// <param name="name">The element event name.</param>
    /// <param name="callback">
    /// A delegate supplies to this event. 
    /// </param>
    /// <param name="builder">The instance of <see cref="FluentRenderTreeBuilder"/>.</param>
    /// <param name="receiver">The receiver of event.</param>
    /// <param name="condition">A condition witch satisfied to add callback.</param>
    /// <typeparam name="TValue">The argument of event.</typeparam>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentAttributeBuilder Callback<TValue>(this IFluentAttributeBuilder builder, string name, object receiver, Func<TValue, Task> callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Event.Create(receiver, callback), condition);
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

    #region ForEach
    /// <summary>
    /// Loop to create element with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">A action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach(this IFluentOpenBuilder builder, string name, int count, Action<IFluentAttributeBuilder, int>? action = default, Condition? condition = default)
    {
        Condition.Execute(condition, () =>
        {
            for ( int i = 0; i < count; i++ )
            {
                var element = builder.Element(name);
                element.Key(i);
                action?.Invoke(element, i);
                element.Close();
            }
        });
        return builder;
    }

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="componentType">The type of component.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">A action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach(this IFluentOpenBuilder builder, Type componentType, int count, Action<IFluentAttributeBuilder, int>? action = default, Condition? condition = default)
    {
        Condition.Execute(condition, () =>
        {
            for ( int i = 0; i < count; i++ )
            {
                var component = builder.Component(componentType);
                component.Key(i);
                action?.Invoke(component, i);
                component.Close();
            }
        });
        return builder;
    }

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">A action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach<TComponent>(this IFluentOpenBuilder builder, int count, Action<IFluentAttributeBuilder, int>? action = default, Condition? condition = default) where TComponent : IComponent
    => builder.ForEach(typeof(TComponent), count, action, condition);
    #endregion
}
