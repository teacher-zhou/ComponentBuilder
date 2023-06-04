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
    /// 丝滑渲染树的 API。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <returns></returns>
    internal static IFluentOpenBuilder Fluent(this RenderTreeBuilder builder) => new FluentRenderTreeBuilder(builder);

    /// <summary>
    /// 丝滑渲染树的 API。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <returns></returns>
    internal static IFluentOpenComponentBuilder<TComponent> Fluent<TComponent>(this RenderTreeBuilder builder) where TComponent : IComponent 
        => new FluentRenderTreeBuilder<TComponent>(builder);

    #region Element

    /// <summary>
    /// 表示满足条件时创建具有指定 HTML 名称的开始元素标记。
    /// </summary>
    /// <param name="name">HTML 元素的名称。</param>
    /// <param name="class">元素的 <c>class</c> 属性的值。</param>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> 实例。</param>
    /// <param name="condition">创建元素所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始元素标记的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Element(this IFluentOpenElementBuilder builder, string name, string? @class = default, Condition? condition = default, int? sequence = default)
        => Condition.Execute(condition, () => builder.Element(name, sequence).Class(@class), () => (FluentRenderTreeBuilder)builder);

    /// <summary>
    /// 表示满足条件时创建具有指定 HTML 名称的开始元素标记。
    /// </summary>
    /// <param name="name">HTML 元素的名称。</param>
    /// <param name="class">元素的 <c>class</c> 属性的值。</param>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="condition">创建元素所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始元素标记的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Element(this RenderTreeBuilder builder, string name, string? @class = default, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Element(name, @class, condition, sequence);
    #endregion

    #region Component
    /// <summary>
    /// 创建指定组件类型的开始标记。
    /// </summary>
    /// <param name="condition">创建组件所满足的条件。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> 实例。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始组件标记的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Component(this IFluentOpenComponentBuilder builder, Type componentType, Condition? condition = default, int? sequence = default)
        => Condition.Execute(condition, () => builder.Component(componentType, sequence), () => (FluentRenderTreeBuilder)builder);

    /// <summary>
    /// 创建指定组件类型的开始标记。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="componentType">组件的类型。</param>
    /// <param name="condition">创建组件所满足的条件。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始组件标记的 <see cref="RenderTreeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Component(this RenderTreeBuilder builder, Type componentType, Condition? condition = default, int? sequence = default)
        => builder.Fluent().Component(componentType, condition, sequence);

    /// <summary>
    /// 创建指定组件类型的开始标记。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="condition">创建组件所满足的条件。</param>
    /// <param name="builder"><see cref="IFluentOpenElementBuilder"/> 实例。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始组件标记的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Component<TComponent>(this IFluentOpenComponentBuilder builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
        => builder.Component(typeof(TComponent), condition, sequence);

    /// <summary>
    /// 创建指定组件类型的开始标记。
    /// </summary>
    /// <typeparam name="TComponent">组件的类型。</typeparam>
    /// <param name="condition">创建组件所满足的条件。</param>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">表示源代码位置的序列。默认为随机生成。</param>
    /// <returns>包含开始组件标记的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    public static IFluentAttributeBuilder<TComponent> Component<TComponent>(this RenderTreeBuilder builder, Condition? condition = default, int? sequence = default)
        where TComponent : IComponent
         => Condition.Execute(condition, () => builder.Fluent<TComponent>().Component(sequence), () => (FluentRenderTreeBuilder<TComponent>)builder.Fluent<TComponent>());

    #endregion

    #region Content
    /// <summary>
    /// 向此元素添加文本字符串。多个内容将被合并用于多个调用   
    /// </summary>
    /// <param name="text">要插入到内部元素中的文本字符串。</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <returns>包含内容字符串的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentContentBuilder Content(this IFluentContentBuilder builder, string? text)
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// 向此元素添加标记字符串。多个内容将被合并用于多个调用   
    /// </summary>
    /// <param name="markup">要插入到内部元素中的文本字符串。</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <returns>包含内容字符串的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentContentBuilder Content(this IFluentContentBuilder builder, MarkupString markup)
        => builder.Content(b => b.AddContent(0, markup));

    /// <summary>
    /// 添加一个带有指定值的片段到内部组件。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <param name="fragment">代码片段。</param>
    /// <param name="value">代码片段所需要的上下文的值。</param>
    /// <returns>包含代码片段的 <see cref="IFluentContentBuilder"/> 实例。</returns>
    public static IFluentContentBuilder Content<TValue>(this IFluentContentBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Content(b => b.AddContent(0, fragment, value));

    /// <summary>
    /// 添加文本字符串。多个内容将被合并用于多个调用。
    /// </summary>
    /// <param name="text">要插入的文本字符串。</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <returns>包含内容字符串的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentOpenBuilder Content(this IFluentOpenBuilder builder, string? text)
        => builder.Content(b => b.AddContent(0, text));
    /// <summary>
    /// 添加标记字符串。多个内容将被合并用于多个调用。
    /// </summary>
    /// <param name="markup">要插入的标记字符串。</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <returns>包含内容字符串的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentOpenBuilder Content(this IFluentOpenBuilder builder, MarkupString markup)
        => builder.Content(b => b.AddContent(0, markup));

    /// <summary>
    /// 添加一个带有指定值的片段到内部组件。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="builder"><see cref="IFluentOpenBuilder"/> 实例。</param>
    /// <param name="fragment">代码片段。</param>
    /// <param name="value">代码片段所需要的上下文的值。</param>
    /// <returns>包含代码片段的 <see cref="IFluentOpenBuilder"/> 实例。</returns>
    public static IFluentOpenBuilder Content<TValue>(this IFluentOpenBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Content(b => b.AddContent(0, fragment, value));


    /// <summary>
    /// 添加文本字符串。多个内容将被合并用于多个调用。
    /// </summary>
    /// <param name="text">要插入的文本字符串。</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <returns>包含内容字符串的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentOpenBuilder Content(this RenderTreeBuilder builder, string? text)
        => builder.Fluent().Content(b => b.AddContent(0, text));
    /// <summary>
    /// 添加标记字符串。多个内容将被合并用于多个调用。
    /// </summary>
    /// <param name="markup">要插入的标记字符串。</param>
    /// <param name="builder"><see cref="IFluentContentBuilder"/> 实例。</param>
    /// <returns>包含内容字符串的 <see cref="IFluentOpenElementBuilder"/> 实例。</returns>
    public static IFluentOpenBuilder Content(this RenderTreeBuilder builder, MarkupString markup)
        => builder.Fluent().Content(b => b.AddContent(0, markup));

    /// <summary>
    /// 添加一个带有指定值的片段到内部组件。
    /// </summary>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="builder"><see cref="IFluentOpenBuilder"/> 实例。</param>
    /// <param name="fragment">代码片段。</param>
    /// <param name="value">代码片段所需要的上下文的值。</param>
    /// <returns>包含代码片段的 <see cref="IFluentOpenBuilder"/> 实例。</returns>
    public static IFluentOpenBuilder Content<TValue>(this RenderTreeBuilder builder, RenderFragment<TValue>? fragment, TValue value)
        => builder.Fluent().Content(b => b.AddContent(0, fragment, value));

    #endregion

    #region Content<TComponent>
    /// <summary>
    /// 将片段内容添加到组件的 ChildContent 参数中。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/> 的实例。</param>
    /// <param name="fragment">代码片段。</param>
    /// <param name="condition">满足添加参数的条件。</param>
    /// <returns>包含参数的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    public static IFluentAttributeBuilder<TComponent> Content<TComponent>(this IFluentAttributeBuilder<TComponent> builder, RenderFragment? fragment, Condition? condition = default)
        where TComponent : IComponent
         => Condition.Execute(condition, () => builder.Content(fragment), () => builder);

    /// <summary>
    /// 将标记字符串添加到组件的 ChildContent 参数中。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/> 的实例。</param>
    /// <param name="markup">标记字符串。</param>
    /// <param name="condition">满足添加参数的条件。</param>
    /// <returns>包含参数的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    public static IFluentAttributeBuilder<TComponent> Content<TComponent>(this IFluentAttributeBuilder<TComponent> builder, MarkupString? markup, Condition? condition = default)
        where TComponent : IComponent
        => builder.Content(HtmlHelper.Instance.CreateContent(markup), condition);

    /// <summary>
    /// 将文本字符串添加到组件的 ChildContent 参数中。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/> 的实例。</param>
    /// <param name="text">文本字符串。</param>
    /// <param name="condition">满足添加参数的条件。</param>
    /// <returns>包含参数的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    public static IFluentAttributeBuilder<TComponent> Content<TComponent>(this IFluentAttributeBuilder<TComponent> builder, string? text, Condition? condition = default)
        where TComponent : IComponent
        => builder.Content(HtmlHelper.Instance.CreateContent(text), condition);
    #endregion

    #region Attribute
    /// <summary>
    /// 当满足条件时，添加元素属性或组件参数和属性。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 的实例。</param>
    /// <param name="condition">满足添加属性的条件。</param>
    /// <param name="name">参数或 HTML 属性的名称。</param>
    /// <param name="value">参数或 HTML 属性的值。</param>
    /// <returns>包含参数或属性的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Attribute<T>(this IFluentAttributeBuilder builder, string name, T? value, Condition? condition = default)
        => Condition.Execute(condition, () => builder.Attribute(name, value), () => builder);

    /// <summary>
    /// 当满足条件时，添加元素属性或组件参数和属性。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 的实例。</param>
    /// <param name="condition">满足添加属性的条件。</param>
    /// <param name="attributes">参数或 HTML 属性的键值对对象。
    /// <para>
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
    /// </para>
    /// </param>
    /// <returns>包含参数或属性的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Attribute(this IFluentAttributeBuilder builder, object attributes, Condition? condition = default)
    {
        HtmlHelper.Instance.MergeHtmlAttributes(attributes)?.ForEach(item => builder.Attribute(item.Key, item.Value, condition));
        return builder;
    }

    /// <summary>
    /// 添加组件的参数和值。
    /// </summary>
    /// <typeparam name="TComponent">组件类型。</typeparam>
    /// <typeparam name="TValue">值的类型。</typeparam>
    /// <param name="builder"></param>
    /// <param name="parameter">参数选择器。</param>
    /// <param name="value">参数的值。</param>
    /// <param name="condition">满足添加参数的条件。</param>
    /// <returns>包含参数的 <see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="parameter"/> 是 <c>null</c>。</exception>
    public static IFluentAttributeBuilder<TComponent> Attribute<TComponent, TValue>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, TValue>> parameter, TValue? value, Condition? condition = default)
        where TComponent : IComponent
        => Condition.Execute(condition, () => builder.Attribute(parameter, value), () => builder);

    #endregion

    #region Class
    /// <summary>
    /// 向 HTML 元素添加 <c>class</c> 属性。
    /// </summary>
    /// <remarks>可以多次调用以追加 class 的内容。</remarks>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 的实例。</param>
    /// <param name="class">要添加的 class 属性的值。</param>
    /// <param name="condition">添加 class 属性所满足的条件。</param>
    /// <returns>包含 class 属性的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Class(this IFluentAttributeBuilder builder, string? @class, Condition? condition = default)
        => @class.IsNotNullOrEmpty() ? builder.Attribute("class", $"{@class} ", condition) : builder;

    #endregion

    #region Style
    /// <summary>
    /// 向 HTML 元素添加 <c>style</c> 属性。
    /// </summary>
    /// <remarks>可以多次调用以追加 style 的内容。</remarks>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 的实例。</param>
    /// <param name="style">要添加的 style 属性的值。</param>
    /// <param name="condition">添加 style 属性所满足的条件。</param>
    /// <returns>包含 style 属性的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder Style(this IFluentAttributeBuilder builder, string? style, Condition? condition = default)
        => style.IsNotNullOrEmpty() ? builder.Attribute("style", $"{style};", condition) : builder;
    #endregion

    #region Callback
    /// <summary>
    /// 向当前元素添加事件名称的回调委托。
    /// </summary>
    /// <param name="name">事件名称。</param>
    /// <param name="callback">这个事件的回调。</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="condition">满足添加回调的条件。</param>
    /// <returns>包含事件回调的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    /// <exception cref="ArgumentException"><paramref name="name"/> is <see langword="null"/> or empty.</exception>
    public static IFluentAttributeBuilder Callback(this IFluentAttributeBuilder builder, [NotNull]string name, EventCallback callback, Condition? condition = default)
        => builder.Attribute(name, callback, condition);

    /// <summary>
    /// 向当前元素添加事件名称的回调委托。
    /// </summary>
    /// <param name="callbackParameter">事件参数。</param>
    /// <param name="callback">这个事件的回调。</param>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="condition">满足添加回调的条件。</param>
    /// <returns>包含事件回调的 <see cref="IFluentAttributeBuilder"/> 实例。</returns>
    public static IFluentAttributeBuilder<TComponent> Callback<TComponent>(this IFluentAttributeBuilder<TComponent> builder, Expression<Func<TComponent, EventCallback>> callbackParameter, EventCallback callback, Condition? condition = default)
        where TComponent : IComponent
        => builder.Attribute(callbackParameter, callback, condition);

    #endregion

    #region Ref
    /// <summary>
    /// 在渲染后捕获组件的引用。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="captureReferenceAction">在呈现组件后捕获组件引用的操作。</param>
    public static IFluentAttributeBuilder Ref<TComponent>(this IFluentAttributeBuilder builder, Action<TComponent?> captureReferenceAction) where TComponent : IComponent
    {
        builder.Ref(obj => captureReferenceAction((TComponent?)obj));
        return builder;
    }

    /// <summary>
    /// 在渲染后捕获组件的引用。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder{TComponent}"/> 实例。</param>
    /// <param name="captureReferenceAction">在呈现组件后捕获组件引用的操作。</param>
    public static IFluentAttributeBuilder<TComponent> Ref<TComponent>(this IFluentAttributeBuilder<TComponent> builder, Action<TComponent?> captureReferenceAction) where TComponent : IComponent
    {
        builder.Ref(obj => captureReferenceAction((TComponent?)obj));
        return builder;
    }

    /// <summary>
    /// 捕获渲染后元素的引用。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="captureReferenceAction">在呈现组件后捕获元素引用的操作。</param>
    public static IFluentAttributeBuilder Ref(this IFluentAttributeBuilder builder, Action<ElementReference?> captureReferenceAction)
    {
        builder.Ref(el => captureReferenceAction((ElementReference?)el));
        return builder;
    }
    #endregion

    #region MultipleAttributes
    /// <summary>
    /// 使用匿名对象向元素或组件添加多个属性。
    /// </summary>
    /// <param name="builder"><see cref="IFluentAttributeBuilder"/> 实例。</param>
    /// <param name="attributes">
    /// 要合并的 HTML 属性或组件参数。
    /// <para>
    /// 支持 <c>IEnumerable&lt;KeyValuePair&lt;string, object>></c> 和匿名类型，例如 <c>new { @class="class1", id="my-id" , onclick = xxx }</c>。
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

    ///// <summary>
    ///// 循环创建具有指定循环次数的组件。
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    ///// <param name="componentType">The type of component.</param>
    ///// <param name="count">The times loop.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach(this RenderTreeBuilder builder, Type componentType, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
    //    => builder.Fluent().ForEach(componentType, count, action, condition);
    ///// <summary>
    ///// Loop to create element with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    ///// <param name="name">The name of element.</param>
    ///// <param name="count">The times loop.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach(this RenderTreeBuilder builder, string name, int count, Action<(IFluentAttributeBuilder builder, int index)>? action = default, Condition? condition = default)
    //    => builder.Fluent().ForEach(name, count, action, condition);

    ///// <summary>
    ///// Loop to create element with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="name">The name of element.</param>
    ///// <param name="count">The times loop.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach(this IFluentOpenBuilder builder, string name, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
    //{
    //    Condition.Execute(condition, () =>
    //    {
    //        for (int i = 0; i < count; i++)
    //        {
    //            var element = builder.Element(name);
    //            element.Key(i);
    //            action?.Invoke(new(element, i));
    //            element.Close();
    //        }
    //    });
    //    return builder;
    //}

    ///// <summary>
    ///// Loop to create component with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="componentType">The type of component.</param>
    ///// <param name="count">The times loop.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach(this IFluentOpenBuilder builder, Type componentType, int count, Action<(IFluentAttributeBuilder attribute, int index)>? action = default, Condition? condition = default)
    //{
    //    Condition.Execute(condition, () =>
    //    {
    //        for (int i = 0; i < count; i++)
    //        {
    //            var component = builder.Component(componentType);
    //            component.Key(i);
    //            action?.Invoke(new(component, i));
    //            component.Close();
    //        }
    //    });
    //    return builder;
    //}


    ///// <summary>
    ///// Loop to create element with specified collection.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="T">The type of collection.</typeparam>
    ///// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    ///// <param name="name">The name of element.</param>
    ///// <param name="collection">The collection to loop if not empty.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach<T>(this RenderTreeBuilder builder, string name, IEnumerable<T> collection, Action<(IFluentAttributeBuilder builder, int index, T item)>? action = default, Condition? condition = default)
    //    => builder.Fluent().ForEach(name, collection, action, condition);

    ///// <summary>
    ///// Loop to create element with specified collection.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="T">The type of collection.</typeparam>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="name">The name of element.</param>
    ///// <param name="collection">The collection to loop if not empty.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach<T>(this IFluentOpenBuilder builder, string name, IEnumerable<T> collection, Action<(IFluentAttributeBuilder attribute, int index, T item)>? action = default, Condition? condition = default)
    //{
    //    Condition.Execute(condition, () =>
    //    {
    //        var index = 0;
    //        foreach ( var item in collection )
    //        {
    //            var i = index;
    //            var element = builder.Element(name);
    //            element.Key(i);
    //            action?.Invoke(new(element, index, item));
    //            element.Close();
    //            index++;
    //        }
    //    });
    //    return builder;
    //}

    ///// <summary>
    ///// Loop to create component with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="T">The type of collection.</typeparam>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="componentType">The type of component.</param>
    ///// <param name="collection">The collection to loop.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach<T>(this IFluentOpenBuilder builder, IEnumerable<T> collection, Type componentType, Action<(IFluentAttributeBuilder attribute, int index, T item)>? action = default, Condition? condition = default)
    //{
    //    Condition.Execute(condition, () =>
    //    {
    //        var index = 0;
    //        foreach ( var item in collection )
    //        {
    //            var i = index;
    //            var component = builder.Component(componentType);
    //            component.Key(i);
    //            action?.Invoke(new(component, index, item));
    //            component.Close();
    //            index++;
    //        }

    //    });
    //    return builder;
    //}

    ///// <summary>
    ///// Loop to create component with specified collection.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="T">The type of collection.</typeparam>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="componentType">The type of component.</param>
    ///// <param name="collection">The collection to loop if not empty.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenBuilder ForEach<T>(this RenderTreeBuilder builder, IEnumerable<T> collection, Type componentType, Action<(IFluentAttributeBuilder builder, int index, T item)>? action = default, Condition? condition = default)
    //    => builder.Fluent().ForEach(collection, componentType, action, condition);

    ///// <summary>
    ///// Loop to create component with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="TComponent">The type of component.</typeparam>
    ///// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    ///// <param name="count">The times loop.</param>
    ///// <param name="action">A action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent>(this RenderTreeBuilder builder, int count, Action<(IFluentAttributeBuilder<TComponent> attribute, int index)>? action = default, Condition? condition = default) where TComponent : IComponent
    //    => builder.Fluent< TComponent>().ForEach<TComponent>(count, action, condition);

    ///// <summary>
    ///// Loop to create component with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="TComponent">The type of component.</typeparam>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="count">The times loop.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent>(this IFluentOpenComponentBuilder<TComponent> builder, int count, Action<(IFluentAttributeBuilder<TComponent> attribute, int index)>? action = default, Condition? condition = default) where TComponent : IComponent
    //=> builder.ForEach(count, action, condition);


    ///// <summary>
    ///// Loop to create component with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="TComponent">The type of component.</typeparam>
    ///// <typeparam name="T">The type of collection.</typeparam>
    ///// <param name="builder">The instance of <see cref="RenderTreeBuilder"/>.</param>
    ///// <param name="collection">The collection to loop if not empty.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent, T>(this RenderTreeBuilder builder, IEnumerable<T> collection, Action<(IFluentAttributeBuilder attribute, int index, T item)>? action = default, Condition? condition = default) where TComponent : IComponent
    //    => builder.Fluent<TComponent>().ForEach<TComponent,Task>(collection,action, condition);

    ///// <summary>
    ///// Loop to create component with specified loop times.
    ///// <para>
    ///// The <see cref="IFluentCloseBuilder.Close"/> is called automatically.
    ///// </para>
    ///// </summary>
    ///// <typeparam name="TComponent">The type of component.</typeparam>
    ///// <typeparam name="T">The type of collection.</typeparam>
    ///// <param name="builder">The instance of <see cref="IFluentOpenBuilder"/>.</param>
    ///// <param name="collection">The collection to loop if not empty.</param>
    ///// <param name="action">An action to do when looping for each item.</param>
    ///// <param name="condition">A condition satisfied to start looping.</param>
    ///// <returns>A <see cref="IFluentAttributeBuilder"/> instance contains event attribute.</returns>
    //public static IFluentOpenComponentBuilder<TComponent> ForEach<TComponent,T>(this IFluentOpenComponentBuilder<TComponent> builder, IEnumerable<T> collection, Action<(IFluentAttributeBuilder<TComponent> attribute, int index, T item)>? action = default, Condition? condition = default) where TComponent : IComponent
    //=> builder.ForEach<TComponent,T>(collection, action, condition);
    #endregion
}
