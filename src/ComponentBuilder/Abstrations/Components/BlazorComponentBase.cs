using System.Reflection;
using Microsoft.JSInterop;
using ComponentBuilder.Abstrations.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentBuilder;

/// <summary>
/// 表示具备自动化组件特性的基类。这是一个抽象类。
/// </summary>
public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent, IRefreshableComponent
{
    /// <summary>
    /// 初始化 <see cref="BlazorComponentBase"/> 类的新实例。
    /// </summary>
    protected BlazorComponentBase()
    {
        CssClassBuilder = ServiceProvider?.GetService<ICssClassBuilder>() ?? new DefaultCssClassBuilder();
        StyleBuilder = ServiceProvider?.GetService<IStyleBuilder>() ?? new DefaultStyleBuilder();
        CurrentComponent = this;
    }

    #region Properties

    protected object CurrentComponent { get; private set; }

    #region Injection
    /// <summary>
    /// 获取 <see cref="IServiceProvider"/> 实例。
    /// </summary>
    [Inject] protected IServiceProvider ServiceProvider { get; set; }

    #endregion Injection

    #region Parameters

    /// <summary>
    /// 获取或设置元素中的附加属性，它可以自动捕获不匹配的 html 属性值。
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// 获取或设置元素中的附加 class 的值。
    /// </summary>
    [Parameter] public string? AdditionalCssClass { get; set; }
    /// <summary>
    /// 获取或设置元素中的附加 style 的值。
    /// </summary>
    [Parameter] public string? AdditionalStyle { get; set; }
    /// <summary>
    /// 提供一个可以有 CSS 类提供器的扩展实例。
    /// </summary>
    [Parameter] public ICssClassUtility CssClass { get; set; }

    #endregion Parameters

    #region Protected
    /// <summary>
    /// 在调用延迟初始化后获取 <see cref="IJSRuntime"/> 的实例。
    /// </summary>
    protected Lazy<IJSRuntime> JS
    {
        get
        {
            var js = ServiceProvider.GetService<IJSRuntime>();
            if (js is not null)
            {
                return new(() => js, LazyThreadSafetyMode.PublicationOnly);
            }
            return new Lazy<IJSRuntime>();
        }
    }

    /// <summary>
    /// 在调用延迟初始化后获取服务器的 WebAssembly 环境支持。
    /// </summary>
    /// <value>如果是 WebAssembly 返回 <c>true</c>, 否则返回 <c>false</c>.</value>
    protected Lazy<bool> IsWebAssembly => new(() => JS.Value is IJSInProcessRuntime);

    /// <summary>
    /// 获取 <see cref="ICssClassBuilder"/> 的实例。
    /// </summary>
    protected ICssClassBuilder CssClassBuilder { get; }
    /// <summary>
    /// 获取 <see cref="IStyleBuilder"/> 的实例。
    /// </summary>
    protected IStyleBuilder StyleBuilder { get; }

    /// <summary>
    /// 可重写以创建当前组件呈现的 HTML 标记名称。默认从 <see cref="HtmlTagAttributeResolver"/> 解析器中解析。
    /// </summary>
    /// <exception cref="InvalidOperationException">标记名称是 is null, 空或空白字符串。</exception>
    protected virtual string TagName
    {
        get
        {
            var tagName = ServiceProvider.GetRequiredService<HtmlTagAttributeResolver>().Resolve(CurrentComponent);
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new InvalidOperationException($"The tag name is null, empty or whitespace.");
            }
            return tagName;
        }
    }

    /// <summary>
    /// 可重写在实例中重写生成启动序列的源代码序列。
    /// </summary>
    protected virtual int RegionSequence => GetHashCode();

    /// <summary>
    /// 获取子组件的集合。
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    /// <summary>
    /// 获取或设置添加子组件时执行的操作。
    /// </summary>
    protected Action<IComponent>? OnComponentAdded { get; set; }
    #endregion

    #region Events    
    /// <summary>
    /// 在构建CSS类之前会引发一个事件。
    /// </summary>
    public event EventHandler<CssClassEventArgs> OnCssClassBuilding;
    /// <summary>
    /// 在CSS类构建完成后会引发一个事件。
    /// </summary>
    public event EventHandler<CssClassEventArgs> OnCssClassBuilt;
    #endregion

    #endregion Properties

    #region Method

    #region Core
    /// <summary>
    /// 如果重写该方法，请显示地调用 <see cref="AddCascadingComponent"/> 以自动识别 <see cref="ChildComponentAttribute"/> 特性作为子组件的自动化调用。
    /// </summary>
    protected override void OnInitialized()
    {
        AddCascadingComponent();
        base.OnInitialized();
    }
    /// <summary>
    /// 如果重写该方法，请显示地调用 <see cref="ResolveAdditionalAttributes"/> 以自动调用 <see cref="IHtmlAttributesResolver"/> 和 <see cref="IHtmlEventAttributeResolver"/> 解析器。
    /// </summary>
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        ResolveAdditionalAttributes();
    }

    /// <summary>
    /// <inheritdoc/>
    /// <para>
    /// 注意: 除非您了解组件创建的所有特性，否则不要重写此方法。
    /// </para>
    /// <para>
    /// 你可以重写以下方法来完成特定逻辑：
    /// </para>
    /// <list type="bullet">
    /// <item>重写 <see cref="AddContent(RenderTreeBuilder, int)"/> 用最少的代码创建特定的内部内容。</item>
    /// <item>如果有必要的话，请重写 <see cref="BuildComponentRenderTree(RenderTreeBuilder)"/> 而不是 <see cref="BuildRenderTree(RenderTreeBuilder)"/> 方法。</item>
    /// </list>
    /// </summary>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenRegion(RegionSequence);
        CreateComponentTree(builder, BuildComponentRenderTree);
        builder.CloseRegion();
    }

    #endregion

    #region Public

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>每一项用空格隔开的字符串。</returns>
    public virtual string? GetCssClassString()
    {
        CssClassBuilder.Dispose();

        if (TryGetClassAttribute(out var value))
        {
            return value;
        }

        OnCssClassBuilding?.Invoke(CurrentComponent, new CssClassEventArgs(CssClassBuilder));

        CssClassBuilder.Append(ServiceProvider.GetService<ICssClassAttributeResolver>()?.Resolve(CurrentComponent));

        BuildCssClass(CssClassBuilder);

        CssClassBuilder.Append(CssClass?.CssClasses ?? Enumerable.Empty<string>())
                        .Append(AdditionalCssClass);

        OnCssClassBuilt?.Invoke(CurrentComponent, new CssClassEventArgs(CssClassBuilder));

        return CssClassBuilder.ToString();
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <returns>每一项都用';'分隔的字符串。</returns>
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
    /// 通知组件状态已更改并重新呈现组件。
    /// </summary>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

    /// <summary>
    /// 向该组件添加子组件。
    /// </summary>
    /// <param name="component">要添加的组件。</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
    public virtual Task AddChildComponent(IComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        ChildComponents.Add(component);
        OnComponentAdded?.Invoke(component);
        return ((IRefreshableComponent)CurrentComponent).Refresh();
    }
    #endregion Public

    #region Protected

    #region Can Override
    /// <summary>
    /// 可重写该方法以实现有逻辑的创建组件的 CSS 类。
    /// </summary>
    /// <param name="builder"><see cref="ICssClassBuilder"/> 的实例。</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder)
    {
    }

    /// <summary>
    /// 可重写该方法以实现有逻辑的创建组件的 style。
    /// </summary>
    /// <param name="builder"><see cref="IStyleBuilder"/> 的实例。</param>
    protected virtual void BuildStyle(IStyleBuilder builder)
    {

    }

    /// <summary>
    /// 可重写该方法以实现有逻辑的创建组件的其他附加属性。
    /// </summary>
    /// <param name="attributes">已经包含了 <see cref="AdditionalAttributes"/> 值的附加属性。</param>
    protected virtual void BuildAttributes(IDictionary<string, object> attributes)
    {

    }

    /// <summary>
    /// 构建组件公共属性并返回源代码的最后一个序列号。
    /// <para>
    /// 方法调用顺序如下：
    /// </para>
    /// <list type="number">
    /// <item>
    /// 调用 <see cref="AddClassAttribute(RenderTreeBuilder, int)"/> 方法;
    /// </item>
    /// <item>
    /// 调用 <see cref="AddStyleAttribute(RenderTreeBuilder, int)"/> 方法;
    /// </item>
    /// <item>
    /// 调用 <see cref="AddMultipleAttributes(RenderTreeBuilder, int)"/> 方法;
    /// </item>
    /// </list>
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    protected virtual void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        AddClassAttribute(builder, 1);
        AddStyleAttribute(builder, 2);
        AddMultipleAttributes(builder, sequence = 3);
    }
    #endregion

    /// <summary>
    /// 代替 <see cref="BuildRenderTree(RenderTreeBuilder)"/> 创建组件树。如果重写该方法，请显示地调用 <see cref="BuildComponentAttributes(RenderTreeBuilder, out int)"/> 和 <see cref="AddContent(RenderTreeBuilder, int)"/> 完成组件的自动化创建。
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    protected virtual void BuildComponentRenderTree(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
    }
    #region AddContent
    /// <summary>
    /// 添加组件的内容框架。
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => AddChildContent(builder, sequence);

    /// <summary>
    /// 如果组件实现 <see cref="IHasChildContent"/> 实例，将自动追加内容。
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (CurrentComponent is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
    }


    /// <summary>
    /// 如果组件实现 <see cref="IHasChildContent{TValue}"/> 实例，将自动追加内容。
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    /// <param name="value">子内容的值。</param>
    protected void AddChildContent<TValue>(RenderTreeBuilder builder, int sequence, TValue value)
    {
        if (CurrentComponent is IHasChildContent<TValue> content)
        {
            builder.AddContent(sequence, content.ChildContent, value);
        }
    }

    #endregion
    /// <summary>
    /// 如果 class 的值不为空，则向组件追加 class 属性。
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    protected void AddClassAttribute(RenderTreeBuilder builder, int sequence)
    {
        var cssClass = GetCssClassString();
        if (!string.IsNullOrEmpty(cssClass))
        {
            builder.AddAttribute(sequence, "class", cssClass);
        }
    }
    /// <summary>
    /// 如果 style 的值不为空，则向组件追加 style 属性。
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    protected void AddStyleAttribute(RenderTreeBuilder builder, int sequence)
    {
        var style = GetStyleString();
        if (!string.IsNullOrEmpty(style))
        {
            builder.AddAttribute(sequence, "style", style);
        }
    }

    /// <summary>
    /// 添加一个框架，以相同的序列号表示多个属性，包括特定指示符的标识，以创建HTML属性。
    /// </summary>
    /// <param name="builder">要追加的 <see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">一个整数，表示该指令在源代码中的位置。</param>
    protected void AddMultipleAttributes(RenderTreeBuilder builder, int sequence)
    {
        builder.AddMultipleAttributes(sequence, AdditionalAttributes);
    }

    /// <summary>
    /// 解析 <see cref="AdditionalAttributes"/> 的值。
    /// </summary>
    protected void ResolveAdditionalAttributes()
    {
        var attributes = new Dictionary<string, object>(AdditionalAttributes).AsEnumerable();

        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributesResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            var value = resolver.Resolve(CurrentComponent);
            attributes = attributes.Merge(value);
        }

        var eventCallbacks = ServiceProvider.GetService<IHtmlEventAttributeResolver>()?.Resolve(CurrentComponent);

        if (eventCallbacks is not null)
        {
            attributes = attributes.Merge(eventCallbacks);
        }

        AdditionalAttributes = new Dictionary<string, object>(attributes.Distinct());
        BuildAttributes(AdditionalAttributes);
    }

    /// <summary>
    /// 尝试从该组件的HTML元素中获取 class 属性。
    /// </summary>
    /// <param name="cssClass">组件的 class 属性的值，该值可能为 <c>null</c>。</param>
    /// <returns>如果 class 属性存在，则返回 <c>true</c> , 否则返回 <c>false</c>。</returns>
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
    /// 尝试从该组件的HTML元素中获取 style 属性。
    /// </summary>
    /// <param name="style">组件的 style 属性的值，该值可能为 <c>null</c>。</param>
    /// <returns>如果 style 属性存在，则返回 <c>true</c> , 否则返回 <c>false</c>。</returns>
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
    /// 将此组件添加到父组件中，该组件的标识 <see cref="ChildComponentAttribute"/> 提供父组件的级联参数。
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
                var propertyValue = property.GetValue(CurrentComponent);

                if (propertyType != attr.ComponentType)
                {
                    continue;
                }
                if (!attr.Optional && propertyValue is null)
                {
                    throw new InvalidOperationException(string.Format("组件 {4} 设置了 {2} 特性，并有一个公开类型是 {1} 的级联参数 {0} ，该特性要求组件必须在 {1} 组件中使用，或设置特性 {2} 的 {3} 为 false 将不抛出此异常", property.Name, attr.ComponentType.Name, nameof(ChildComponentAttribute), nameof(ChildComponentAttribute.Optional), componentType.Name));
                }

                if (propertyType is not null && propertyValue is not null)
                {
                    ((Task)propertyType!.GetMethod(nameof(AddChildComponent))!
                        .Invoke(propertyValue!, new[] { CurrentComponent }))!.GetAwaiter().GetResult();
                }
            }
        }
    }
    #endregion

    #region Private


    /// <summary>
    /// 创建组件树。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="continoues">创建完成后要继续执行的操作。</param>
    private void CreateComponentTree(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
    {
        var componentType = GetType();

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

            genericMethod.Invoke(null, new object[] { builder, CurrentComponent, 0, content, parentComponent.Name!, parentComponent.IsFixed });
        }


        /// <summary>
        /// 创建组件或元素。
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="continoues">创建完成后要继续执行的操作。</param>
        /// <exception cref="InvalidOperationException"></exception>
        void CreateComponentOrElement(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
        {
            var componentType = GetType();

            //if (componentType.IsDefined(typeof(ServiceComponentAttribute)))
            //{
            //    var regitsterComponent = ServiceProvider.GetService(componentType);
            //    if (regitsterComponent is null)
            //    {
            //        throw new InvalidOperationException($"组件 '{componentType.Name}' 定义了 {nameof(ServiceComponentAttribute)} 特性，表示该组件要求添加作为服务才可以使用，请先调用 'builder.Services.RegisterComponent' 方法注册组件");
            //    }

            //    //复制参数
            //    foreach (var serviceComponentProperty in CurrentComponent.GetType().GetProperties().Where(m => m.CanRead))
            //    {
            //        foreach (var currentComponentProperty in regitsterComponent.GetType().GetProperties().Where(m => m.CanWrite))
            //        {
            //            if (serviceComponentProperty.Name == currentComponentProperty.Name)
            //            {
            //                currentComponentProperty.SetValue(regitsterComponent, serviceComponentProperty.GetValue(CurrentComponent));
            //            }
            //        }
            //    }

            //    //参数，子类存在，父类不存在的，会被捕获到 AdditionalAttributes 中，对比名称如果是一样的，则赋值
            //    foreach (var attribute in this.AdditionalAttributes)
            //    {
            //        try
            //        {
            //            var property = regitsterComponent.GetType().GetProperty(attribute.Key);
            //            if (property is not null)
            //            {
            //                property.SetValue(regitsterComponent, attribute.Value);
            //                this.AdditionalAttributes.Remove(attribute.Key);
            //            }
            //        }
            //        catch (AmbiguousMatchException)//没找到这个属性，则忽略
            //        {
            //            continue;
            //        }
            //    }

            //    CurrentComponent = regitsterComponent;
            //    builder.OpenComponent(0, CurrentComponent.GetType());
            //    //continoues(builder);
            //    builder.CloseComponent();
            //    return;
            //}

            if (componentType.TryGetCustomAttribute<RenderComponentAttribute>(out var renderComponentAttribute))
            {
                if (renderComponentAttribute!.ComponentType == GetType())
                {
                    throw new InvalidOperationException($"当前组件和 {nameof(RenderComponentAttribute)} 特性定义的组件{renderComponentAttribute.ComponentType.Name} 不能是同一个组件，这会导致死循环！！！");
                }
                componentType = renderComponentAttribute!.ComponentType;

                builder.OpenComponent(0, componentType);
                continoues(builder);
                builder.CloseComponent();
                return;
            }

            ////当前组件的父组件是否为 ServiceComponent
            //if (componentType.BaseType!.IsDefined(typeof(ServiceComponentAttribute), false))
            //{
            //    //把基类的属性全部 copy 到当前组件中

            //    var serviceComponent = (BlazorComponentBase)ServiceProvider.GetService(componentType.BaseType);
            //    var baseComponentType = serviceComponent.GetType();

            //    //复制参数
            //    foreach (var serviceComponentProperty in baseComponentType.GetProperties().Where(m => m.CanRead))
            //    {
            //        foreach (var currentComponentProperty in componentType.GetProperties().Where(m => m.CanWrite))
            //        {
            //            if (serviceComponentProperty.Name == currentComponentProperty.Name)
            //            {
            //                currentComponentProperty.SetValue(CurrentComponent, serviceComponentProperty.GetValue(serviceComponent));
            //            }
            //        }
            //    }

            //    //参数，子类存在，父类不存在的，会被捕获到 AdditionalAttributes 中，对比名称如果是一样的，则赋值
            //    //foreach (var attribute in serviceComponent.AdditionalAttributes)
            //    //{
            //    //    try
            //    //    {
            //    //        componentType.GetProperty(attribute.Key)?.SetValue(CurrentComponent, attribute.Value);
            //    //    }
            //    //    catch (AmbiguousMatchException)//没找到这个属性，则忽略
            //    //    {
            //    //        continue;
            //    //    }
            //    //}
            //}


            builder.OpenElement(0, TagName);
            continoues(builder);
            builder.CloseElement();
        }
    }

    #endregion
    #endregion
}
