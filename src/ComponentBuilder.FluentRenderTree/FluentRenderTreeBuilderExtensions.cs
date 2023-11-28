using System.Xml.Linq;
using System.Security.Claims;
using System.Linq.Expressions;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder.FluentRenderTree;
/// <summary>
/// The extensions of <see cref="RenderTreeBuilder"/> for <see cref="FluentRenderTreeBuilder"/>
/// </summary>
public static class FluentRenderTreeBuilderExtensions
{
    /// <summary>
    /// Silky render tree API.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> </param>
    /// <returns></returns>
    public static IFluentOpenBuilder Fluent(this RenderTreeBuilder builder) => new FluentRenderTreeBuilder(builder);

    /// <summary>
    /// Silky render tree API.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> </param>
    /// <returns></returns>
    public static IFluentOpenComponentBuilder<TComponent> Fluent<TComponent>(this RenderTreeBuilder builder) where TComponent : IComponent 
        => new FluentRenderTreeBuilder<TComponent>(builder);

    #region Element

    /// <summary>
    /// Indicates that a start element tag with the specified HTML name is created when the condition is met.
    /// </summary>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> instance.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="class">The <c>class</c> value.</param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    public static IFluentAttributeBuilder Element(this IFluentOpenElementBuilder builder, string name, string? @class = default, Condition? condition = default, int? sequence = default)
        => Condition.Execute(condition, () => builder.Element(name, sequence).Class(@class), () => (FluentRenderTreeBuilder)builder);

    /// <summary>
    /// Indicates that a start element tag with the specified HTML name is created when the condition is met.
    /// </summary>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> instance.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="class">The <c>class</c> value.</param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    public static IFluentAttributeBuilder Element(this RenderTreeBuilder builder, string name, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Element(name, @class, condition, sequence);
    #endregion

    #region Component
    /// <summary>
    /// Creates the start tag for the specified component type.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="componentType">The component type.</param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    /// <returns>包含开始组件标记的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Component(this IFluentOpenComponentBuilder builder, Type componentType, Condition? condition = default, int? sequence = default)
        => Condition.Execute(condition, () => builder.Component(componentType, sequence), () => (FluentRenderTreeBuilder)builder);

    /// <summary>
    /// Creates the start tag for the specified component type.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="componentType">The component type.</param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    /// <returns>包含开始组件标记的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Component(this RenderTreeBuilder builder, Type componentType, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Component(componentType, condition, sequence);

    /// <summary>
    /// Creates the start tag for the specified component type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder"></param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    public static IFluentAttributeBuilder<TComponent> Component<TComponent>(this IFluentOpenComponentBuilder builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
    {
        var fluentBuilder = ((FluentRenderTreeBuilder)builder);
        fluentBuilder.Close();

        return fluentBuilder.Builder.Component<TComponent>(condition, sequence);
    }

    /// <summary>
    /// Creates the start tag for the specified component type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder"></param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    public static IFluentAttributeBuilder<TComponent> Component<TComponent>(this IFluentOpenComponentBuilder<TComponent> builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
        => Condition.Execute(condition, () => builder.Component(sequence), () => (FluentRenderTreeBuilder<TComponent>)builder);


    /// <summary>
    /// Creates the start tag for the specified component type.
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder"></param>
    /// <param name="condition">The condition that the element is created to satisfy.</param>
    /// <param name="sequence">A sequence representing the location of the source code. The default value is random generation.</param>
    public static IFluentAttributeBuilder<TComponent> Component<TComponent>(this RenderTreeBuilder builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
         => Condition.Execute(condition, () => builder.Fluent<TComponent>().Component(sequence), () => (FluentRenderTreeBuilder<TComponent>)builder.Fluent<TComponent>());

    #endregion

    #region Content
    /// <summary>
    /// Add a text string to this element.   
    /// </summary>
    /// <param name="text">A string of text to insert into the inner element.</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> </param>
    public static IFluentContentBuilder Content(this IFluentContentBuilder builder, string? text)
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// Add a markup string to this element.   
    /// </summary>
    /// <param name="markup">A string of markup to insert into the inner element.</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> </param>
    public static IFluentContentBuilder Content(this IFluentContentBuilder builder, MarkupString markup)
        => builder.Content(b => b.AddContent(0, markup));

    /// <summary>
    /// Adds a fragment with the specified value to the internal component.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="builder"><see cref="IFluentContentBuilder"/></param>
    /// <param name="fragment">The fragment.</param>
    /// <param name="value">The value of the context required by the code snippet.</param>
    public static IFluentContentBuilder Content<TValue>(this IFluentContentBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Content(b => b.AddContent(0, fragment, value));

    /// <summary>
    /// Add a text string to this element.   
    /// </summary>
    /// <param name="text">A string of text to insert into the inner element.</param>
    /// <param name="builder"><see cref="IFluentOpenBuilder"/> </param>
    public static IFluentOpenBuilder Content(this IFluentOpenBuilder builder, string? text)
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// Add a markup string to this element.   
    /// </summary>
    /// <param name="markup">A string of markup to insert into the inner element.</param>
    /// <param name="builder"><see cref="IFluentOpenBuilder"/> </param>
    public static IFluentOpenBuilder Content(this IFluentOpenBuilder builder, MarkupString markup)
        => builder.Content(b => b.AddContent(0, markup));

    /// <summary>
    /// Adds a fragment with the specified value to the internal component.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="builder"><see cref="IFluentOpenBuilder"/></param>
    /// <param name="fragment">The fragment.</param>
    /// <param name="value">The value of the context required by the code snippet.</param>
    public static IFluentOpenBuilder Content<TValue>(this IFluentOpenBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Content(b => b.AddContent(0, fragment, value));

    /// <summary>
    /// Adds a fragment.
    /// </summary>
    /// <param name="fragment">The content fragment to insert the inner element.</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/></param>
    public static IFluentOpenBuilder Content(this RenderTreeBuilder builder, RenderFragment? fragment)
        => builder.Fluent().Content(fragment);

    /// <summary>
    /// Add a text string to this element.   
    /// </summary>
    /// <param name="text">A string of text to insert into the inner element.</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> </param>
    public static IFluentOpenBuilder Content(this RenderTreeBuilder builder, string? text)
        => builder.Fluent().Content(b => b.AddContent(0, text));
    /// <summary>
    /// Add a markup string to this element.   
    /// </summary>
    /// <param name="markup">A string of markup to insert into the inner element.</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> </param>
    public static IFluentOpenBuilder Content(this RenderTreeBuilder builder, MarkupString markup)
        => builder.Fluent().Content(b => b.AddContent(0, markup));

    /// <summary>
    /// Adds a fragment with the specified value to the internal component.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    /// <param name="builder"><see cref="IFluentOpenBuilder"/></param>
    /// <param name="fragment">The fragment.</param>
    /// <param name="value">The value of the context required by the code snippet.</param>
    public static IFluentOpenBuilder Content<TValue>(this RenderTreeBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Fluent().Content(b => b.AddContent(0, fragment, value));

    #endregion

    #region Content<TComponent>
    /// <summary>
    /// Adds the fragment content to the component's <c>ChildContent</c> parameter.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/></param>
    /// <param name="fragment">The fragment.</param>
    /// <param name="condition">The conditions for adding parameters are met.</param>
    public static IFluentAttributeBuilder<TComponent> Content<TComponent>(this IFluentAttributeBuilder<TComponent> builder, RenderFragment? fragment, Condition? condition = default)
        where TComponent : IComponent
         => Condition.Execute(condition, () => builder.Content(fragment), () => builder);

    /// <summary>
    /// Adds the markup string to the component's <c>ChildContent</c> parameter.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/></param>
    /// <param name="markup">The markup string.</param>
    /// <param name="condition">The conditions for adding parameters are met.</param>
    public static IFluentAttributeBuilder<TComponent> Content<TComponent>(this IFluentAttributeBuilder<TComponent> builder, MarkupString? markup, Condition? condition = default)
        where TComponent : IComponent
        => builder.Content(HtmlHelper.Instance.CreateContent(markup), condition);

    /// <summary>
    /// Adds the text string to the component's <c>ChildContent</c> parameter.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/></param>
    /// <param name="text">The text content.</param>
    /// <param name="condition">The conditions for adding parameters are met.</param>
    public static IFluentAttributeBuilder<TComponent> Content<TComponent>(this IFluentAttributeBuilder<TComponent> builder, string? text, Condition? condition = default)
        where TComponent : IComponent
        => builder.Content(HtmlHelper.Instance.CreateContent(text), condition);
    #endregion

    #region Attribute
    /// <summary>
    /// When the conditions are met, add element attributes or component parameters and attributes.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/></param>
    /// <param name="condition">The conditions for adding an attribute are met.</param>
    /// <param name="name">The name of the parameter or HTML attribute.</param>
    /// <param name="value">The value of a parameter or HTML attribute.</param>
    public static IFluentAttributeBuilder Attribute<T>(this IFluentAttributeBuilder builder, string name, T? value, Condition? condition = default)
        => Condition.Execute(condition, () => builder.Attribute(name, value), () => builder);


    /// <summary>
    /// When the conditions are met, add element attributes or component parameters and attributes.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/></param>
    /// <param name="condition">The conditions for adding an attribute are met.</param>
    /// <param name="attributes">The value of a parameter or HTML attribute.</param>
    public static IFluentAttributeBuilder Attribute(this IFluentAttributeBuilder builder, object attributes, Condition? condition = default)
    {
        HtmlHelper.Instance.MergeHtmlAttributes(attributes)?.ForEach(item => builder.Attribute(item.Key, item.Value, condition));
        return builder;
    }

    /// <summary>
    /// Add parameter and value for the component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    /// <param name="builder"></param>
    /// <param name="parameter">Parameter selector.</param>
    /// <param name="value">Parameter value.</param>
    /// <param name="condition">The conditions for adding parameters are met.</param>
    /// <exception cref="ArgumentNullException"><paramref name="parameter"/> is <c>null</c>。</exception>
    public static IFluentAttributeBuilder<TComponent> Attribute<TComponent, TValue>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, TValue>> parameter, TValue? value, Condition? condition = default)
        where TComponent : IComponent
        => Condition.Execute(condition, () => builder.Attribute(parameter, value), () => builder);

    #endregion

    #region Class
    /// <summary>
    /// Add the <c>class</c> attribute to the HTML element.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="class">The value of class.</param>
    /// <param name="condition">Add the condition that the class attribute meets.</param>
    public static IFluentAttributeBuilder Class(this IFluentAttributeBuilder builder, string? @class, Condition? condition = default)
        => @class.IsNotNullOrEmpty() ? builder.Attribute("class", $"{@class} ", condition) : builder;

    #endregion

    #region Style
    /// <summary>
    /// Add the <c>style</c> attribute to the HTML element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="style">The value of style.</param>
    /// <param name="condition">Add the condition that the style attribute meets.</param>
    public static IFluentAttributeBuilder Style(this IFluentAttributeBuilder builder, string? style, Condition? condition = default)
        => style.IsNotNullOrEmpty() ? builder.Attribute("style", $"{style};", condition) : builder;
    #endregion

    #region Callback

    #region IFluentAttributeBuilder
    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name">Event name.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, [NotNull]string name, EventCallback callback, Condition? condition = default)
        => builder.Attribute(name, callback, condition);
    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="builder"></param>
    /// <param name="name">Event name.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback<TEventArgs>(this IFluentAttributeBuilder builder, [NotNull] string name, EventCallback<TEventArgs> callback, Condition? condition = default)
        => builder.Attribute(name, callback, condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="name">Event name.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, [NotNull] string name, object receiver, Action callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="name">Event name.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback<TEventArgs>(this IFluentAttributeBuilder builder, [NotNull] string name, object receiver, Action callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="name">Event name.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback<TEventArgs>(this IFluentAttributeBuilder builder, [NotNull] string name, object receiver,Action<TEventArgs> callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="name">Event name.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, [NotNull] string name, object receiver, Func<Task> callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current element.
    /// </summary>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="name">Event name.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback for event.</param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback<TEventArgs>(this IFluentAttributeBuilder builder, [NotNull] string name, object receiver, Func<TEventArgs, Task> callback, Condition? condition = default)
        => builder.Callback(name, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);
    #endregion

    #region IFluentAttributeBuilder<TComponent>
    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback>> callbackParameter, EventCallback callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, callback, condition);


    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent, TEventArgs>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback<TEventArgs>>> callbackParameter, EventCallback<TEventArgs> callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, callback, condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback>> callbackParameter, object receiver, Action callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);

    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent,TEventArgs>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback<TEventArgs>>> callbackParameter, object receiver, Action callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, HtmlHelper.Instance.Callback().Create<TEventArgs>(receiver, callback), condition);


    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent, TEventArgs>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback<TEventArgs>>> callbackParameter, object receiver, Action<TEventArgs> callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);


    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback>> callbackParameter, object receiver, Func<Task> callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, HtmlHelper.Instance.Callback().Create(receiver, callback), condition);


    /// <summary>
    /// A callback delegate that adds the event name to the current component.
    /// </summary>
    /// <typeparam name="TComponent">Component type.</typeparam>
    /// <typeparam name="TEventArgs">Event argument type.</typeparam>
    /// <param name="callbackParameter">Event parameter.</param>
    /// <param name="receiver">Event receiver.</param>
    /// <param name="callback">The callback to this event.</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="condition">The conditions for adding a callback are met.</param>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent, TEventArgs>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback<TEventArgs>>> callbackParameter, object receiver, Func<TEventArgs,Task> callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, HtmlHelper.Instance.Callback().Create<TEventArgs>(receiver, callback), condition);
    #endregion

    #endregion

    #region Ref
    /// <summary>
    /// Capture a reference to the component after rendering.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="captureReferenceAction">Capture the operations referenced by the component after rendering it.</param>
    public static IFluentAttributeBuilder Ref<TComponent>(this IFluentAttributeBuilder builder, Action<TComponent?> captureReferenceAction) where TComponent : IComponent
    {
        builder.Ref(obj => captureReferenceAction((TComponent?)obj));
        return builder;
    }
    /// <summary>
    /// Capture a reference to the component after rendering.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/> </param>
    /// <param name="captureReferenceAction">Capture the operations referenced by the component after rendering it.</param>
    public static IFluentAttributeBuilder<TComponent> Ref<TComponent>(this IFluentAttributeBuilder<TComponent> builder, Action<TComponent?> captureReferenceAction) where TComponent : IComponent
    {
        builder.Ref(obj => captureReferenceAction((TComponent?)obj));
        return builder;
    }

    /// <summary>
    /// Capture a reference to the rendered element。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> </param>
    /// <param name="captureReferenceAction">Actions that capture element references after rendering the component.</param>
    public static IFluentAttributeBuilder Ref(this IFluentAttributeBuilder builder, Action<ElementReference?> captureReferenceAction)
    {
        builder.Ref(el => captureReferenceAction((ElementReference?)el));
        return builder;
    }
    #endregion

    #region MultipleAttributes
    /// <summary>
    /// Use anonymous objects to add multiple properties to an element or component.
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="attributes">
    /// HTML attributes or component parameters to merge.
    /// <para>
    /// Support <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> and anonymous object，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <exception cref="ArgumentNullException"><paramref name="attributes"/> 是 null.</exception>
    public static IFluentAttributeBuilder MultipleAttributes(this IFluentAttributeBuilder builder, object? attributes)
    {
        if ( attributes is null )
        {
            throw new ArgumentNullException(nameof(attributes));
        }

        builder.MultipleAttributes(HtmlHelper.Instance.MergeHtmlAttributes(attributes)!);

        return builder;
    }
    #endregion

    #region ForEach

    #region Element
    /// <summary>
    /// Loop to create element with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach(this RenderTreeBuilder builder, string name, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
        => builder.Fluent().ForEach(name, count, action, condition);

    /// <summary>
    /// Loop to create element with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach(this IFluentOpenBuilder builder, string name, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
    {
        Condition.Execute(condition, () =>
        {
            for (int i = 0; i < count; i++)
            {
                var element = builder.Element(name);
                element.Key(i);
                action?.Invoke(new(element, i));
                element.Close();
            }
        });
        return builder;
    }

    /// <summary>
    /// Loop to create element with specified collection.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="collection">The collection to loop if not empty.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach<T>(this RenderTreeBuilder builder, string name, IEnumerable<T> collection, Action<(IFluentAttributeBuilder builder, int index, T item)>? action = default, Condition? condition = default)
        => builder.Fluent().ForEach(name, collection, action, condition);

    /// <summary>
    /// Loop to create element with specified collection.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="name">The name of element.</param>
    /// <param name="collection">The collection to loop if not empty.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach<T>(this IFluentOpenBuilder builder, string name, IEnumerable<T> collection, Action<(IFluentAttributeBuilder attribute, int index, T item)>? action = default, Condition? condition = default)
    {
        Condition.Execute(condition, () =>
        {
            var index = 0;
            foreach (var item in collection)
            {
                var i = index;
                var element = builder.Element(name);
                element.Key(i);
                action?.Invoke(new(element, index, item));
                element.Close();
                index++;
            }
        });
        return builder;
    }
    #endregion

    #region Component

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    /// <param name="componentType">The type of component.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach(this RenderTreeBuilder builder, Type componentType, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
        => builder.Fluent().ForEach(componentType, count, action, condition);
    

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="componentType">The type of component.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach(this IFluentOpenBuilder builder, Type componentType, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
    {
        Condition.Execute(condition, () =>
        {
            for (int i = 0; i < count; i++)
            {
                var component = builder.Component(componentType);
                component.Key(i);
                action?.Invoke(new(component, i));
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
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="componentType">The type of component.</param>
    /// <param name="collection">The collection to loop.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach<T>(this IFluentOpenBuilder builder, IEnumerable<T> collection, Type componentType, Action<(IFluentAttributeBuilder attribute, int index, T item)>? action = default, Condition? condition = default)
    {
        Condition.Execute(condition, () =>
        {
            var index = 0;
            foreach (var item in collection)
            {
                var i = index;
                var component = builder.Component(componentType);
                component.Key(i);
                action?.Invoke(new(component, index, item));
                component.Close();
                index++;
            }

        });
        return builder;
    }

    /// <summary>
    /// Loop to create component with specified collection.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="componentType">The type of component.</param>
    /// <param name="collection">The collection to loop if not empty.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenBuilder ForEach<T>(this RenderTreeBuilder builder, IEnumerable<T> collection, Type componentType, Action<(IFluentAttributeBuilder builder, int index, T item)>? action = default, Condition? condition = default)
        => builder.Fluent().ForEach(collection, componentType, action, condition);

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">A action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent>(this RenderTreeBuilder builder, int count, Action<(IFluentAttributeBuilder<TComponent> attribute, int index)>? action = default, Condition? condition = default) where TComponent : IComponent
        => builder.Fluent<TComponent>().ForEach(count, action, condition);

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="count">The times loop.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent>(this IFluentOpenComponentBuilder<TComponent> builder, int count, Action<(IFluentAttributeBuilder<TComponent> attribute, int index)>? action = default, Condition? condition = default) where TComponent : IComponent
    {
        Condition.Execute(condition, () =>
        {
            for (int i = 0; i < count; i++)
            {
                var component = builder.Component();
                component.Key(i);
                action?.Invoke(new(component, i));
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
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    /// <param name="collection">The collection to loop if not empty.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent, T>(this RenderTreeBuilder builder, IEnumerable<T> collection, Action<(IFluentAttributeBuilder<TComponent> attribute, int index, T item)>? action = default, Condition? condition = default) where TComponent : IComponent
        => builder.Fluent<TComponent>().ForEach(collection, action, condition);

    /// <summary>
    /// Loop to create component with specified loop times.
    /// <para>
    /// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    /// </para>
    /// </summary>
    /// <typeparam name="TComponent">The type of component.</typeparam>
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    /// <param name="collection">The collection to loop if not empty.</param>
    /// <param name="action">An action to do when looping for each item.</param>
    /// <param name="condition">A condition satisfied to start looping.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent, T>(this IFluentOpenComponentBuilder<TComponent> builder, IEnumerable<T> collection, Action<(IFluentAttributeBuilder<TComponent> attribute, int index, T item)>? action = default, Condition? condition = default) where TComponent : IComponent
    {
        Condition.Execute(condition, () =>
        {
            var index = 0;
            foreach (var item in collection)
            {
                var i = index;
                var component = builder.Component();
                component.Key(i);
                action?.Invoke(new(component, index, item));
                component.Close();
                index++;
            }

        });
        return builder;
    }
    #endregion
    #endregion
}
