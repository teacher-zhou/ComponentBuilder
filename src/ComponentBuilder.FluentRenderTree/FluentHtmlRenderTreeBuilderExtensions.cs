namespace ComponentBuilder.FluentRenderTree;

/// <summary>
/// <see cref="IFluentOpenBuilder"/> 的扩展。
/// </summary>
public static class FluentHtmlRenderTreeBuilderExtensions
{
    #region Element

    #region Div
    /// <summary>
    /// 创建 <c>&lt;div></c> 的 HTML 元素开始标记.
    /// </summary>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> 实例。</param>
    /// <param name="class">元素的 <c>class</c> 属性的值。</param>
    /// <param name="condition">创建元素所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始元素标记的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Div(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("div", @class, condition, sequence);
    /// <summary>
    /// 创建 <c>&lt;div></c> 的 HTML 元素开始标记。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="class">元素的 <c>class</c> 属性的值。</param>
    /// <param name="condition">创建元素所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始元素标记的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Div(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Div(@class, condition, sequence);
    #endregion

    #region Span
    /// <summary>
    /// 创建 <c>&lt;span></c> 的 HTML 元素开始标记.
    /// </summary>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> 实例。</param>
    /// <param name="class">元素的 <c>class</c> 属性的值。</param>
    /// <param name="condition">创建元素所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始元素标记的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Span(this IFluentOpenBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Element("span", @class, condition, sequence);

    /// <summary>
    /// 创建 <c>&lt;span></c> 的 HTML 元素开始标记.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="class">元素的 <c>class</c> 属性的值。</param>
    /// <param name="condition">创建元素所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始元素标记的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Span(this RenderTreeBuilder builder, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Span(@class, condition, sequence);
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

    //#region Input
    ///// <summary>
    ///// 创建 <c>&lt;input /></c> 的 HTML 元素开始标记。
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <param name="value">The value of input.</param>
    ///// <param name="type">The type of input element.</param>
    ///// <param name="class">CSS class to add this element.</param>
    ///// <param name="condition">A condition satisfied to add element.</param>
    ///// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    //public static IFluentAttributeBuilder Input(this IFluentOpenBuilder builder, object? value, string? type = "text", string? @class = default, Condition? condition = default, int? sequence = default)
    //=> builder.Element("input", @class, condition, sequence).Attribute("type", type).Attribute("value", value);

    ///// <summary>
    ///// Create <c>&lt;input type="xxx" /></c> element.
    ///// </summary>
    ///// <param name="builder"></param>
    ///// <param name="value">The value of input.</param>
    ///// <param name="type">The type of input element.</param>
    ///// <param name="class">CSS class to add this element.</param>
    ///// <param name="condition">A condition satisfied to add element.</param>
    ///// <param name="sequence">A sequence representing position of source code. Default to generate randomly.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains element.</returns>
    //public static IFluentAttributeBuilder Input(this RenderTreeBuilder builder, object? value, string? type = "text", string? @class = default, Condition? condition = default, int? sequence = default)
    //    => builder.Fluent().Input(value, type, @class, condition, sequence);

    //#endregion

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
