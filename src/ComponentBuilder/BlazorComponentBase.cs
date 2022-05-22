using System.Reflection;

using ComponentBuilder.Abstrations.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace ComponentBuilder;

/// <summary>
/// 表示能快速创建基于已有 CSS 类库的 Blazor 组件的基类。
/// </summary>
public abstract partial class BlazorComponentBase : ComponentBase, IBlazorComponent, IRefreshComponent
{
    /// <summary>
    /// 初始化 <see cref="BlazorComponentBase"/> 类的新实例。
    /// </summary>
    protected BlazorComponentBase()
    {
        CssClassBuilder = ServiceProvider?.GetService<ICssClassBuilder>() ?? new DefaultCssClassBuilder();
        StyleBuilder = ServiceProvider?.GetService<IStyleBuilder>() ?? new DefaultStyleBuilder();
    }

    #region Properties

    #region Injection
    /// <summary>
    /// 获取 <see cref="IServiceProvider"/> 的实例。
    /// </summary>
    [Inject] protected IServiceProvider ServiceProvider { get; set; }

    #endregion Injection

    #region Parameters

    /// <summary>
    /// 获取或设置自定义参数或 HTML 元素的属性。
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// 获取或设置额外追加的 CSS 字符串。
    /// </summary>
    [Parameter] public string AdditionalCssClass { get; set; }
    /// <summary>
    /// 获取或设置额外追加的 style 字符串。
    /// </summary>
    [Parameter] public string AdditionalStyle { get; set; }
    /// <summary>
    /// 获取或设置 CSS 类。该参数使用 <see cref="Css"/> 类进行扩展和调用。
    /// </summary>
    [Parameter] public ICssClassUtility CssClass { get; set; }

    #endregion Parameters

    #region Protected
    /// <summary>
    /// 获取 <see cref="IJSRuntime"/> 的实例。
    /// </summary>
    protected IJSRuntime? JS
    {
        get
        {
            var js = ServiceProvider.GetService<IJSRuntime>();
            return IsWebAssembly ? js as IJSInProcessRuntime : js;
        }
    }
    /// <summary>
    /// 获取一个布尔值，表示当前模式是 WebAssembly 还是 ServerSide。
    /// </summary>
    /// <value><c>true</c> is WebAssembly, otherwise <c>false</c>.</value>
    protected bool IsWebAssembly => JS is IJSInProcessRuntime;

    /// <summary>
    /// 获取 <see cref="ICssClassBuilder"/> 的实例。
    /// </summary>
    protected ICssClassBuilder CssClassBuilder { get; }
    /// <summary>
    /// 获取 <see cref="IStyleBuilder"/> 的实例。
    /// </summary>
    protected IStyleBuilder StyleBuilder { get; }
    /// <summary>
    /// 获取 HTML 元素的标签。
    /// </summary>
    protected virtual string TagName => ServiceProvider.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);

    /// <summary>
    /// 获取一个随机数，用于创建 <see cref="RenderTreeBuilder"/> 的开始范围。该属性可以有效避免渲染数的序列重复。
    /// </summary>
    protected virtual int RegionSequence => this.GetHashCode();

    /// <summary>
    /// 获取该组件包含的子组件集合。
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    /// <summary>
    /// 获取或设置一个委托，当子组件被添加到父组件时触发。
    /// </summary>
    protected Action<IComponent>? OnComponentAdded { get; set; }
    #endregion

    #region Events    
    /// <summary>
    /// An event will be raised before build CSS classes.
    /// </summary>
    public event EventHandler<CssClassEventArgs> OnCssClassBuilding;
    /// <summary>
    /// An event will be raised after CSS classes has been built.
    /// </summary>
    public event EventHandler<CssClassEventArgs> OnCssClassBuilt;
    #endregion

    #endregion Properties

    #region Method

    #region Core
    /// <summary>
    /// 重写初始化组件，自动新增级联组件。
    /// </summary>
    protected override void OnInitialized()
    {
        AddCascadingComponent();
        base.OnInitialized();
    }

    /// <summary>
    /// 注意：如果重写，将失去该框架的特性，除非你对内部很熟悉。
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenRegion(RegionSequence);

        CreateComponentTree(builder, BuildComponentRenderTree);

        builder.CloseRegion();
    }

    #endregion

    #region Public

    /// <summary>
    /// 返回组件参数最终所需要的 CSS 类名称字符串。
    /// </summary>
    /// <returns>由空格分隔的一系列 CSS 类名称的字符串</returns>
    public virtual string? GetCssClassString()
    {
        CssClassBuilder.Dispose();

        if (TryGetClassAttribute(out var value))
        {
            return value;
        }

        OnCssClassBuilding?.Invoke(this, new CssClassEventArgs(CssClassBuilder));

        CssClassBuilder.Append(ServiceProvider.GetService<ICssClassAttributeResolver>()?.Resolve(this));

        BuildCssClass(CssClassBuilder);

        CssClassBuilder.Append(CssClass?.CssClasses ?? Enumerable.Empty<string>())
                        .Append(AdditionalCssClass);

        OnCssClassBuilt?.Invoke(this, new CssClassEventArgs(CssClassBuilder));

        return CssClassBuilder.ToString();
    }

    /// <summary>
    /// 返回组件最终所需要的 style 样式的字符串。
    /// </summary>
    /// <returns>由“;”分隔的一系列 style 样式的字符串。</returns>
    public virtual string? GetStyleString()
    {
        StyleBuilder.Dispose();

        if (TryGetStyleAttribute(out string? value))
        {
            return value;
        }

        BuildStyle(StyleBuilder);

        if (!string.IsNullOrWhiteSpace(AdditionalStyle))
        {
            StyleBuilder.Append(AdditionalStyle);
        }
        return StyleBuilder.ToString();
    }

    /// <summary>
    /// 通知组件其状态已更改。如果适用，这将导致组件被重新渲染。
    /// </summary>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

    /// <summary>
    /// 添加子组件
    /// </summary>
    /// <param name="component">要添加的组件</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
    public virtual Task AddChildComponent(IComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        ChildComponents.Add(component);
        OnComponentAdded?.Invoke(component);
        return this.Refresh();
    }
    #endregion Public

    #region Protected

    #region Can Override
    /// <summary>
    /// Overrides to build CSS class by special logical process.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder)
    {
    }

    /// <summary>
    /// Overrides to build style by special logical process.
    /// </summary>
    /// <param name="builder">The instance of <see cref="IStyleBuilder"/>.</param>
    protected virtual void BuildStyle(IStyleBuilder builder)
    {

    }

    /// <summary>
    /// Overrides to build additional attributes by special logical process.
    /// </summary>
    /// <param name="attributes">The attributes contains all resolvers to build attributes and <see cref="AdditionalAttributes"/>.</param>
    protected virtual void BuildAttributes(IDictionary<string, object> attributes)
    {

    }

    /// <summary>
    /// 构建组件的属性。派生类不建议重写该方法，除非你熟悉创建渲染树的结构。
    /// <para>
    /// 构建顺序如下：
    /// </para>
    /// <list type="number">
    /// <item>
    /// 调用 <see cref="AddClassAttribute(RenderTreeBuilder, int)"/> 方法；
    /// </item>
    /// <item>
    /// 调用 <see cref="AddStyleAttribute(RenderTreeBuilder, int)"/> 方法；
    /// </item>
    /// <item>
    /// 调用 <see cref="AddMultipleAttributes(RenderTreeBuilder, int)"/> 方法。
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="builder">A <see cref="RenderTreeBuilder"/> to create component.</param>
    /// <param name="sequence">An integer that represents the last position of the instruction in the source code.</param>
    protected virtual void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        AddClassAttribute(builder, 1);
        AddStyleAttribute(builder, 2);
        AddMultipleAttributes(builder, sequence = 3);
    }
    #endregion

    /// <summary>
    /// 替代 <see cref="BuildRenderTree(RenderTreeBuilder)"/> 方法来构造组件渲染树。
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    protected virtual void BuildComponentRenderTree(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
    }
    #region AddContent
    /// <summary>
    /// 向组件追加任务内容片段。
    /// </summary>
    /// <param name="builder">要追加片段的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在渲染树源代码中的位置。</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => AddChildContent(builder, sequence);

    /// <summary>
    /// 自动追加实现了 <see cref="IHasChildContent"/> 接口的任意内容片段。
    /// </summary>
    /// <param name="builder">要追加片段的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在渲染树源代码中的位置。</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (this is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
        //if (typeof(IHasChildContent<>).IsAssignableFrom(this.GetType()))
        //{
        //    var componentType = GetType();
        //    var childContentInterfaceType = componentType.GetInterfaces().First(m => m == typeof(IHasChildContent<>));


        //}
    }

    /// <summary>
    /// 自动追加实现了 <see cref="IHasChildContent{TValue}"/> 接口的任意内容片段。
    /// </summary>
    /// <param name="builder">要追加片段的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在渲染树源代码中的位置。</param>
    /// <param name="value">用于构建内容的值。</param>
    protected void AddChildContent<TValue>(RenderTreeBuilder builder, int sequence, TValue value)
    {
        if (this is IHasChildContent<TValue> content)
        {
            builder.AddContent<TValue>(sequence, content.ChildContent, value);
        }


    }

    #endregion
    /// <summary>
    /// 添加组件的 class 属性。
    /// </summary>
    /// <param name="builder">要追加片段的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在渲染树源代码中的位置。</param>
    protected void AddClassAttribute(RenderTreeBuilder builder, int sequence)
    {
        var cssClass = GetCssClassString();
        if (!string.IsNullOrEmpty(cssClass))
        {
            builder.AddAttribute(sequence, "class", cssClass);
        }
    }

    /// <summary>
    /// 添加组件的 style 属性。
    /// </summary>
    /// <param name="builder">要追加片段的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在渲染树源代码中的位置。</param>
    protected void AddStyleAttribute(RenderTreeBuilder builder, int sequence)
    {
        var style = GetStyleString();
        if (!string.IsNullOrEmpty(style))
        {
            builder.AddAttribute(sequence, "style", style);
        }
    }

    /// <summary>
    /// 添加表示具有相同序列号的多个属性的帧。
    /// </summary>
    /// <param name="builder">要追加片段的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在渲染树源代码中的位置。</param>
    protected void AddMultipleAttributes(RenderTreeBuilder builder, int sequence)
    {
        var attributes = new Dictionary<string, object>().AsEnumerable();

        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributesResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            attributes = attributes.Concat(resolver.Resolve(this));
        }

        var eventCallbacks = ServiceProvider.GetService<IHtmlEventAttributeResolver>()?.Resolve(this);
        attributes = attributes.Concat(eventCallbacks);

        if (AdditionalAttributes is not null)
        {
            attributes = attributes.Concat(HtmlHelper.MergeHtmlAttributes(AdditionalAttributes));
        }

        BuildAttributes(AdditionalAttributes);
        attributes = attributes.Concat(AdditionalAttributes);

        builder.AddMultipleAttributes(sequence, attributes.Distinct());
    }

    /// <summary>
    /// 尝试获取组件的 'class' 属性的值。
    /// </summary>
    /// <param name="cssClass">输出该组件 'class' 属性的值，可能为 <c>null</c>。</param>
    /// <returns>若获取到 'class' 属性，则返回 <c>true</c> 否则返回 <c>false</c>。</returns>
    protected bool TryGetClassAttribute(out string? cssClass)
    {
        cssClass = string.Empty;
        if (AdditionalAttributes.TryGetValue("class", out object? value))
        {
            cssClass = value?.ToString();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 尝试获取组件的 'style' 属性的值。
    /// </summary>
    /// <param name="style">输出该组件 'style' 属性的值，可能为 <c>null</c>。</param>
    /// <returns>若获取到 'style' 属性，则返回 <c>true</c> 否则返回 <c>false</c>。</returns>
    protected bool TryGetStyleAttribute(out string? style)
    {
        style = string.Empty;
        if (AdditionalAttributes.TryGetValue("style", out object? value))
        {
            style = value?.ToString();
            return true;
        }
        return false;
    }


    /// <summary>
    /// 自动将标识为 <see cref="CascadingParameterAttribute"/> 组件创建级联参数。
    /// </summary>
    protected void AddCascadingComponent()
    {
        var componentType = GetType();

        var cascadingComponentAttributes = componentType.GetCustomAttributes<ChildComponentAttribute>();
        ;
        if (cascadingComponentAttributes is null)
        {
            return;
        }

        foreach (var attr in cascadingComponentAttributes)
        {
            foreach (var property in componentType.GetProperties().Where(m => m.IsDefined(typeof(CascadingParameterAttribute))))
            {
                var propertyType = property.PropertyType;
                var propertyValue = property.GetValue(this);

                if (propertyType != attr.ComponentType)
                {
                    continue;
                }
                if (!attr.Optional && propertyValue is null)
                {
                    throw new InvalidOperationException($"The value of property '{property.Name}' with type of '{propertyType.Name}' is null, which means component '{componentType.Name}' must be the child of component '{attr.ComponentType.Name}'. Set {nameof(ChildComponentAttribute.Optional)} is true for '{nameof(ChildComponentAttribute)}' in current component can solve this issue");
                }

                if (propertyType is not null && propertyValue is not null)
                {
                    ((Task)propertyType.GetMethod(nameof(AddChildComponent))
                        .Invoke(propertyValue, new[] { this })).GetAwaiter().GetResult();
                }
            }
        }
    }
    #endregion

    #region Private


    private void CreateComponentOrElement(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
    {

        var renderComponentAttribute = this.GetType().GetCustomAttribute<ComponentRenderAttribute>();

        var hasComponentAttr = renderComponentAttribute is not null;

        if (hasComponentAttr)
        {
            if (renderComponentAttribute.ComponentType == GetType())
            {
                throw new InvalidOperationException($"Cannot create self component of {renderComponentAttribute.ComponentType.Name}");
            }
            builder.OpenComponent(0, renderComponentAttribute.ComponentType);
        }
        else
        {
            builder.OpenElement(0, TagName ?? "div");
        }

        continoues(builder);

        if (hasComponentAttr)
        {
            builder.CloseComponent();
        }
        else
        {
            builder.CloseElement();
        }
    }
    private void CreateComponentTree(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
    {
        var componentType = this.GetType();

        var parentComponent = componentType.GetCustomAttribute<ParentComponentAttribute>();
        if (parentComponent is null)
        {
            CreateComponentOrElement(builder, continoues);
        }
        else
        {
            var extensionType = typeof(RenderTreeBuilderExtensions);

            var methods = extensionType.GetMethods()
                .Where(m => m.Name == nameof(RenderTreeBuilderExtensions.CreateCascadingComponent));


            var method = methods.FirstOrDefault();
            if (method is null)
            {
                return;
            }

            var genericMethod = method.MakeGenericMethod(componentType);

            RenderFragment content = new(content =>
            {
                CreateComponentOrElement(content, _ => continoues(content));
            });

            genericMethod.Invoke(null, new object[] { builder, this, 0, content, parentComponent.Name, parentComponent.IsFixed });
        }
    }

    #endregion
    #endregion
}
