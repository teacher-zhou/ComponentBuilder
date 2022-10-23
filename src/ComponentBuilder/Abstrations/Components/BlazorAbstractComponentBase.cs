using System.Reflection;

using ComponentBuilder.Abstrations.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class with automated component features. This is an abstract class.
/// </summary>
public abstract class BlazorAbstractComponentBase : ComponentBase, IBlazorComponent, IRefreshableComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorAbstractComponentBase"/> class.
    /// </summary>
    protected BlazorAbstractComponentBase()
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
    [Inject] protected IServiceProvider? ServiceProvider { get; set; }

    #endregion Injection

    #region Parameters

    /// <summary>
    /// Gets or sets an additional attribute in an element that automatically captures unmatched html attribute values.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    #endregion Parameters

    #region Protected
    /// <summary>
    /// Get <see cref="IJSRuntime"/> instance after lazy initialized.
    /// </summary>
    protected Lazy<IJSRuntime> JS
    {
        get
        {
            var js = ServiceProvider?.GetService<IJSRuntime>();
            if (js is not null)
            {
                return new(() => js, LazyThreadSafetyMode.PublicationOnly);
            }
            return new Lazy<IJSRuntime>();
        }
    }

    /// <summary>
    /// Get the server's WebAssembly environment support after invocation latency initialization.
    /// </summary>
    /// <value><c>true</c> to WebAssembly, otherwise, <c>false</c>.</value>
    protected Lazy<bool> IsWebAssembly => new(() => JS.Value is IJSInProcessRuntime);

    /// <summary>
    /// Gets <see cref="ICssClassBuilder"/> instance.
    /// </summary>
    protected ICssClassBuilder CssClassBuilder { get; }
    /// <summary>
    /// Gets <see cref="IStyleBuilder"/> instance.
    /// </summary>
    protected IStyleBuilder StyleBuilder { get; }

    /// <summary>
    /// Overrides to create HTML tag name for component.
    /// </summary>
    /// <exception cref="InvalidOperationException">The tag name is null, empty or whitespace.</exception>
    protected virtual string TagName
    {
        get
        {
            var tagName = ServiceProvider?.GetRequiredService<HtmlTagAttributeResolver>().Resolve(CurrentComponent);
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new InvalidOperationException($"The tag name is null, empty or whitespace.");
            }
            return tagName;
        }
    }

    /// <summary>
    /// Overrides the source code sequence that generates the startup sequence in the instance.
    /// </summary>
    protected virtual int RegionSequence => GetHashCode();

    /// <summary>
    /// Gets a collection of child components
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    /// <summary>
    /// Gets or sets the action to take when a child component is added.
    /// </summary>
    protected Action<IComponent>? OnComponentAdded { get; set; }
    #endregion

    #endregion Properties

    #region Method

    #region Core
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    protected override void OnInitialized()
    {
        OnComponentInitialized();
    }

    /// <inheritdoc/>
    protected override Task OnInitializedAsync()
    {
        return OnComponentInitializedAsync();
    }

    /// <summary>
    /// Method invoked when componen is ready to start, 
    /// and automatically to create cascading component that defined <see cref="ParentComponentAttribute"/> class.
    /// </summary>
    protected virtual Task OnComponentInitializedAsync()
    {
        OnComponentInitialized();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Method invoked when componen is ready to start, 
    /// and automatically to create cascading component that defined <see cref="ParentComponentAttribute"/> class.
    /// </summary>
    protected virtual void OnComponentInitialized()
    {
        CssClassBuilder.Dispose();
        AddCascadingComponent();
    }

    /// <inheritdoc/>
    protected override void OnParametersSet()
    {
        OnComponentParameterSet();
        base.OnParametersSet();
    }

    /// <inheritdoc/>
    protected override Task OnParametersSetAsync() => OnComponentParameterSetAsync();

    /// <summary>
    /// Method invoke to resolve attributes automatically for parameters.
    /// </summary>
    protected virtual void OnComponentParameterSet() => ResolveHtmlAttributes();

    /// <summary>
    /// Method invoke to resolve attributes automatically for parameters.
    /// </summary>
    protected virtual Task OnComponentParameterSetAsync()
    {
        OnComponentParameterSet();
        return Task.CompletedTask;
    }


    /// <summary>
    /// Automatically build component by ComponentBuilder with new region. 
    /// <para>
    /// NOTE: Override to build component by yourself, or recommend override <see cref="BuildComponentRenderTree(RenderTreeBuilder)"/> instead.
    /// </para>
    /// </summary>
    /// 
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> .</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenRegion(RegionSequence);
        CreateComponentTree(builder, b =>
        {
            BuildComponentAttributes(b, out var sequence);
            AddContent(b, sequence + 2);
        });
        builder.CloseRegion();
    }

    #endregion

    #region Public

    /// <inheritdoc/>
    public virtual string? GetCssClassString()
    {
        //CssClassBuilder.Dispose();

        if (AdditionalAttributes.TryGetValue("class", out object? value))
        {
            return value?.ToString();
        }

        CssClassBuilder.Append(ServiceProvider.GetRequiredService<ICssClassAttributeResolver>()!.Resolve(CurrentComponent));

        BuildCssClass(CssClassBuilder);

        if (this is IHasCssClassUtility cssClassUtility)
        {
            CssClassBuilder.Append(cssClassUtility.CssClass.CssClasses ?? Enumerable.Empty<string>());
        }

        if (this is IHasAdditionalCssClass additionalCssClass)
        {
            CssClassBuilder.Append(additionalCssClass.AdditionalCssClass);
        }

        return CssClassBuilder.ToString();
    }

    /// <inheritdoc/>
    public virtual string? GetStyleString()
    {
        StyleBuilder.Dispose();

        if (AdditionalAttributes.TryGetValue("style", out object? value))
        {
            return value?.ToString();
        }

        BuildStyle(StyleBuilder);

        if (this is IHasAdditionalStyle additionalStyle)
        {
            StyleBuilder.Append(additionalStyle.AdditionalStyle);
        }

        return StyleBuilder.ToString();
    }

    /// <summary>
    /// 通知组件状态已更改并重新呈现组件。
    /// </summary>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

    /// <summary>
    /// Add a component to current component be child component.
    /// </summary>
    /// <param name="component">A component to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
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
    /// Overrides to build CSS class for component customization.
    /// <para>
    /// You can build CSS class of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="builder">A <see cref="ICssClassBuilder"/> instance.</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder)
    {
    }

    /// <summary>
    /// Overrides to build style for component customization.
    /// <para>
    /// You can build style of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="builder">A <see cref="IStyleBuilder"/> instance.</param>
    protected virtual void BuildStyle(IStyleBuilder builder)
    {

    }


    /// <summary>
    /// Overrides to build attributes for component customization.
    /// <para>
    /// You can build attributes of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="attributes">The attributes for components.</param>
    protected virtual void BuildAttributes(IDictionary<string, object> attributes)
    {

    }

    /// <summary>
    /// Build component attributes to supplies <see cref="RenderTreeBuilder"/> instance.
    /// <para>
    /// NOTE: Override may lose all features of ComponentBuild framework unless you understande properly.
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the last sequence of source code.</param>
    protected virtual void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        builder.AddClassAttribute(1, GetCssClassString());
        builder.AddStyleAttribute(2, GetStyleString());
        AddMultipleAttributes(builder, sequence = 3);

    }
    #endregion

    #region AddContent
    /// <summary>
    /// Add innter content to component.
    /// <para>
    /// Implement from <see cref="IHasChildContent"/> interface to add ChildContent automatically.
    /// </para>
    /// <para>
    /// Normally, override this method to build html structures.
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence)
        => AddChildContent(builder, sequence);

    /// <summary>
    /// Add ChildContent parameter to this component if <see cref="IHasChildContent"/> is implemented.
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (CurrentComponent is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
    }

    #endregion

    #region AddMultipleAttributes
    /// <summary>
    /// Add <see cref="AdditionalAttributes"/> parameter to supplied <see cref="RenderTreeBuilder"/> instance.
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    private void AddMultipleAttributes(RenderTreeBuilder builder, int sequence)
        => builder.AddMultipleAttributes(sequence, AdditionalAttributes);
    #endregion

    /// <summary>
    /// Resolve HTML attributes.
    /// </summary>
    protected void ResolveHtmlAttributes()
    {
        var capturedAttributes = new Dictionary<string, object>().AsEnumerable();

        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributesResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            var value = resolver.Resolve(CurrentComponent);
            capturedAttributes = capturedAttributes.Merge(value);
        }

        var eventCallbacks = ServiceProvider.GetService<IHtmlEventAttributeResolver>()?.Resolve(CurrentComponent);

        if (eventCallbacks is not null)
        {
            capturedAttributes = capturedAttributes.Merge(eventCallbacks);
        }

        AdditionalAttributes = new Dictionary<string, object>(capturedAttributes.Distinct());
        BuildAttributes(AdditionalAttributes);
    }

    /// <summary>
    /// Add cascading parameter when <see cref="ChildComponentAttribute"/> is defined in this component class.
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
    /// Create the component tree.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="continoues">An action after component or element is created.</param>
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
