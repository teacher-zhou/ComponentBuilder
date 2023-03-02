namespace ComponentBuilder.Fluent;

/// <summary>
/// The extension methods of <see cref="IFluentOpenBuilder"/> for HTML elements.
/// </summary>
public static class FluentHtmlRenderTreeBuilderExtensions
{
    #region Element

    #region Div
    /// <summary>
    /// Create <c>&lt;div>...&lt;/div></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Div(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("div", @class, condition, sequence);
    /// <summary>
    /// Create <c>&lt;div>...&lt;/div></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Div(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Div(@class, condition, sequence);
    #endregion

    #region Span
    /// <summary>
    /// Create <c>&lt;span>...&lt;/span></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Span(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("span", @class, condition, sequence);

    /// <summary>
    /// Create <c>&lt;span>...&lt;/span></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Span(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Span(@class, condition, sequence);
    #endregion

    #region Ul
    /// <summary>
    /// Create <c>&lt;ul>...&lt;/ul></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Ul(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("ul", @class, condition, sequence);
    /// <summary>
    /// Create <c>&lt;ul>...&lt;/ul></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Ul(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Ul(@class, condition, sequence);
    #endregion

    #region Li
    /// <summary>
    /// Create <c>&lt;li>...&lt;/li></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Li(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("li", @class, condition, sequence);

    /// <summary>
    /// Create <c>&lt;li>...&lt;/li></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Li(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Li(@class, condition, sequence);
    #endregion

    #region Anchor
    /// <summary>
    /// Create <c>&lt;a>...&lt;/a></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="href">The link of anchor.</param>
    /// <param name="target">The style to open link.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Anchor(this IFluentOpenBuilder builder, string? href = default, string? @class = default, Condition? condition = default, string? target = "_blank", int? sequence = default)
        => builder.Element("a", @class, condition, sequence)
                    .Attribute("href", href, href.IsNotNullOrEmpty())
                    .Attribute("target", target, target.IsNotNullOrEmpty())
        ;

    /// <summary>
    /// Create <c>&lt;a>...&lt;/a></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="href">The link of anchor.</param>
    /// <param name="target">The style to open link.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Anchor(this RenderTreeBuilder builder, string? href = default, string? @class = default, Condition? condition = default, string? target = "_blank", int? sequence = default)
        => builder.Fluent().Anchor(href,@class,condition,target, sequence);
    #endregion

    #region Break
    /// <summary>
    /// Create <c>&lt;br /></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Break(this IFluentOpenBuilder builder, Condition? condition, int? sequence = default)
        => builder.Element("br",condition: condition,sequence: sequence);

    /// <summary>
    /// Create <c>&lt;br /></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Break(this RenderTreeBuilder builder, Condition? condition, int? sequence = default)
        =>builder.Fluent().Break(condition, sequence);
    #endregion

    #region Paragraph
    /// <summary>
    /// Create <c>&lt;p>...&lt;/p></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Paragraph(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("p", @class, condition, sequence);

    /// <summary>
    /// Create <c>&lt;p>...&lt;/p></c> element.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="class">CSS class to add this element.</param>
    /// <param name="condition">A condition satisfied to add element.</param>
    /// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Paragraph(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Paragraph(@class, condition, sequence);
    #endregion

    #endregion

    #region Attributes

    /// <summary>
    /// Add <c>aria-{name}="{value}"</c> HTML attribute.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name">The name of aria.</param>
    /// <param name="value">The value of this attribute.</param>
    /// <param name="condition">A condition satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Aria(this IFluentAttributeBuilder builder, string name, object? value, Condition? condition = default)
        => builder.Attribute($"aria-{name}", value, condition);

    /// <summary>
    /// Add <c>data-{name}="{value}"</c> HTML attribute.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name">The name of data.</param>
    /// <param name="value">The value of this attribute.</param>
    /// <param name="condition">A condition satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Data(this IFluentAttributeBuilder builder, string name, object? value, Condition? condition = default)
        => builder.Attribute($"data-{name}", value, condition);

    /// <summary>
    /// Add role="{value}" HTML attribute.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="value">The value of role attribute.</param>
    /// <param name="condition">A condition satisfied to add attribute.</param>
    /// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    public static IFluentAttributeBuilder Role(this IFluentAttributeBuilder builder, object? value, Condition? condition = default)
        => builder.Attribute("role", value, condition);
    #endregion
}
