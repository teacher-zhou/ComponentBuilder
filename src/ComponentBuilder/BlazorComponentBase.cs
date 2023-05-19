using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// 表示具有自动化组件特性的基类。这是一个抽象类。
/// </summary>
public abstract partial class BlazorComponentBase : ComponentBase, IBlazorComponent
{
    #region Contructor
    /// <summary>
    /// 初始化 <see cref="BlazorComponentBase"/> 类的新实例。
    /// </summary>
    protected BlazorComponentBase() : base()
    {
        AdditionalAttributes = new Dictionary<string, object>();
    }
    #endregion

    #region Properties
    /// <inheritdoc/>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

    /// <inheritdoc/>
    public ICssClassBuilder CssClassBuilder { get; private set; }
    /// <inheritdoc/>
    public IStyleBuilder StyleBuilder { get; private set; }
    /// <summary>
    /// 获取 <see cref="IServiceProvider"/> 实例。
    /// </summary>
    [Inject][NotNull] protected IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// 获取组件拦截器集合。
    /// </summary>
    IEnumerable<IComponentInterceptor> Interceptors { get; set; }

    /// <inheritdoc/>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    #endregion

    #region Lifecyle

    #region Initialize
    /// <summary>
    /// 初始化组件。该方法必须在 <see cref="SetParametersAsync(ParameterView)"/> 调用。
    /// </summary>
    protected void Initialize()
    {
        Interceptors ??= ServiceProvider!.GetServices(typeof(IComponentInterceptor)).OfType<IComponentInterceptor>().OrderBy(m => m.Order).AsEnumerable();
        CssClassBuilder = ServiceProvider!.GetRequiredService<ICssClassBuilder>();
        StyleBuilder = ServiceProvider!.GetRequiredService<IStyleBuilder>();
    }
    #endregion

    #region SetParametersAsync
    /// <summary>
    /// 该方法手动执行了 <see cref="ParameterView.SetParameterProperties(object)"/> 传递参数。并且加入了拦截器功能。
    /// 可重写 <see cref="AfterSetParameters(ParameterView)"/> 方法来个性化传入的参数，而不是重写该方法。
    /// </summary>
    /// <param name="parameters">接收到的组件参数。</param>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        Initialize();
        InvokeSetParametersInterceptors(parameters);
        AfterSetParameters(parameters);

        return base.SetParametersAsync(ParameterView.Empty);
    }

    /// <summary>
    /// 提供一个可以在 <see cref="SetParametersAsync(ParameterView)"/> 完事儿后的方法。
    /// </summary>
    /// <param name="parameters">接收到的组件参数。</param>
    protected virtual void AfterSetParameters(ParameterView parameters) { }
    #endregion

    #region OnInitialized

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <remarks>
    /// 如果重写该方法，请手动调用 <see cref="InvokeOnInitializeInterceptors"/> 方法手动执行拦截器功能。
    /// </remarks>
    protected override void OnInitialized() => InvokeOnInitializeInterceptors();
    #endregion

    #region OnParameterSet
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <remarks>
    /// 如果重写该方法，请手动调用 <see cref="InvokeOnParameterSetInterceptors"/> 方法手动执行拦截器功能。
    /// </remarks>
    protected override void OnParametersSet() => InvokeOnParameterSetInterceptors();
    #endregion

    #region OnAfterRender   
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <remarks>
    /// 如果重写该方法，请手动调用 <see cref="InvokeOnAfterRenderInterceptors"/> 方法手动执行拦截器功能。
    /// </remarks>
    protected override void OnAfterRender(bool firstRender)
    {
        if ( firstRender )
        {
            NotifyRenderChildComponent();
        }

        InvokeOnAfterRenderInterceptors(firstRender);
    }

    #endregion

    #endregion

    #region Interceptors

    #region InvokeSetParametersInterceptors
    /// <summary>
    /// 将执行渲染器的 <see cref="IComponentInterceptor.InterceptOnSetParameters(IBlazorComponent, in ParameterView)"/> 方法。
    /// </summary>
    /// <param name="parameters">组件接收到的参数。</param>
    protected void InvokeSetParametersInterceptors(ParameterView parameters)
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnSetParameters(this, parameters);
        }

    }
    #endregion

    #region InvokeOnInitializeInterceptors
    /// <summary>
    /// 将执行 <see cref="IComponentInterceptor.InterceptOnInitialized(IBlazorComponent)"/> 方法。
    /// </summary>
    protected void InvokeOnInitializeInterceptors()
    {
        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnInitialized(this);
        }
    }
    #endregion

    #region InvokeOnParameterSetInterceptors
    /// <summary>
    /// 将执行 <see cref="IComponentInterceptor.InterceptOnParameterSet(IBlazorComponent)"/> 方法。
    /// </summary>
    protected void InvokeOnParameterSetInterceptors()
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnParameterSet(this);
        }
    }
    #endregion

    #region InvokeOnAfterRenderInterceptors
    /// <summary>
    /// 将执行 <see cref="IComponentInterceptor.InterceptOnAfterRender(IBlazorComponent, in bool)"/> 方法。
    /// </summary>
    /// <param name="firstRender"><c>True</c> to indicate component is first render, otherwise, <c>false</c>.</param>
    protected void InvokeOnAfterRenderInterceptors(bool firstRender)
    {
        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnAfterRender(this, firstRender);
        }
    }
    #endregion

    #region InvokeOnBuildContentIntercepts
    /// <summary>
    /// 将执行 <see cref="IComponentInterceptor.InterceptOnContentBuilding(IBlazorComponent, RenderTreeBuilder, int)"/> 方法。
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void InvokeOnBuildContentInterceptors(RenderTreeBuilder builder, int sequence)
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnContentBuilding(this, builder, sequence);
        }
    }
    #endregion

    #region InvokeOnDispose
    /// <summary>
    /// 将执行 <see cref="IComponentInterceptor.InterceptOnDisposing(IBlazorComponent)"/> 方法。
    /// </summary>
    private void InvokeOnDispose()
    {
        if (Interceptors is null)
        {
            return;
        }

        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnDisposing(this);
        }
    }
    #endregion

    #endregion

    #region Build Class/Style/Attributes

    #region BuildCssClass
    /// <summary>
    /// 以逻辑代码的形式构建组件所需要的 CSS 类。
    /// </summary>
    /// <param name="builder"><see cref="ICssClassBuilder"/> 实例。</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder) { }
    #endregion

    #region BuildStyle
    /// <summary>
    /// 以逻辑代码的形式构建组件所需要的 style 样式。
    /// </summary>
    /// <param name="builder"><see cref="IStyleBuilder"/> 实例。</param>
    protected virtual void BuildStyle(IStyleBuilder builder) { }
    #endregion

    #region BuildAttributes
    /// <summary>
    /// 以逻辑代码的形式构建组件所需要的 HTML 属性。
    /// </summary>
    /// <param name="attributes">组件的属性包括从框架解析的和捕获的不匹配的参数。</param>
    protected virtual void BuildAttributes(IDictionary<string, object> attributes) { }
    #endregion

    #endregion

    #region NotifyStateChanged
    /// <inheritdoc/>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
    #endregion

    #region GetCssClassString
    /// <inheritdoc/>
    public string? GetCssClassString()
    {
        var resolvers = ServiceProvider.GetServices<IParameterClassResolver>();
        foreach ( var item in resolvers )
        {
            var result = item.Resolve(this);
            CssClassBuilder.Append(result);
        }

        BuildCssClass(CssClassBuilder);

        return CssClassBuilder.ToString();
    }
    #endregion

    #region GetStyleString
    /// <inheritdoc/>
    public string? GetStyleString()
    {
        StyleBuilder.Clear();

        this.BuildStyle(StyleBuilder);

        if (this is IHasAdditionalStyle additionalStyle)
        {
            StyleBuilder.Append(additionalStyle.AdditionalStyle);
        }

        return StyleBuilder.ToString();
    }
    #endregion

    #region AddChildComponent
    private bool _isChildComponentsAddingCompleted;
    /// <summary>
    /// 通知该组件在添加子组件后应该重新呈现。
    /// </summary>
    protected void NotifyRenderChildComponent()
    {
        if ( _isChildComponentsAddingCompleted )
        {
            StateHasChanged();
            _isChildComponentsAddingCompleted = false;
        }
    }
    /// <summary>
    /// 如果当前组件不存在，向当前组件添加一个组件，使其成为子组件。
    /// </summary>
    /// <param name="component">要添加的组件。</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> 是 null。</exception>
    public virtual void AddChildComponent(IBlazorComponent component)
    {
        if ( component is null )
        {
            throw new ArgumentNullException(nameof(component));
        }

        ChildComponents.Add(component);
        _isChildComponentsAddingCompleted = true;
    }
    #endregion

    #region Dispose
    private bool _disposedValue;

    /// <summary>
    /// 释放组件的资源。
    /// </summary>
    protected virtual void DisposeComponentResources()
    {

    }

    /// <summary>
    /// 释放托管的资源。
    /// </summary>
    protected virtual void DisposeManagedResouces()
    {

    }

    /// <summary>
    /// 执行组件资源的释放。
    /// </summary>
    /// <param name="disposing">如果要释放托管资源，则为 <c>true</c>。</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                DisposeManagedResouces();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null

            DisposeComponentResources();
            InvokeOnDispose();

            CssClassBuilder?.Clear();
            StyleBuilder?.Clear();

            _disposedValue = true;
        }
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~BlazorComponentBase()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    /// <inheritdoc/>
    void IDisposable.Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Automation For Component Class Only

    #region Property
    /// <summary>
    /// 获取或设置一个布尔值，表示是否捕获 HTML 元素的引用。
    /// </summary>
    protected bool CaptureReference { get; set; }

    /// <summary>
    /// 当 <see cref="CaptureReference"/> 为 <c>true</c> 时，将获得 HTML 元素的引用。
    /// </summary>
    public ElementReference? Reference { get; protected set; }
    #endregion

    #region Method

    #region GetRegionSequence
    /// <summary>
    /// 覆盖生成 RenderTreeBuilder 启动序列的源代码序列。
    /// </summary>
    protected virtual int GetRegionSequence() => new Random().Next(100, 999);
    #endregion

    #region GetTagName

    /// <inheritdoc/>
    public virtual string GetTagName()
        => ServiceProvider!.GetRequiredService<IHtmlTagAttributeResolver>()!.Resolve(this);
    #endregion

    #region BuildRenderTree
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="InvalidOperationException">没有任何渲染器。</exception>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var renderers = ServiceProvider.GetServices<IComponentRender>().OfType<IComponentRender>();

        if ( !renderers.Any() )
        {
            throw new InvalidOperationException("没有找到任何渲染器，必须至少提供一个");
        }

        foreach ( var item in renderers )
        {
            if ( !item.Render(this, builder) )
            {
                break;
            }
        }
    }
    #endregion

    #region BuildComponentAttributes
    /// <summary>
    /// 为指定的渲染树构建 HTML 和组件属性。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">返回表示源代码最后一个序列的整数。</param>
    protected void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence) => builder.AddMultipleAttributes(sequence = 4, GetAttributes());
    #endregion

    #region CaptureElementReference
    /// <summary>
    /// 当 <see cref="CaptureReference"/> 是 <c>true</c> 时自动捕获元素的引用并在组件渲染后给 <see cref="Reference"/> 赋值。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">返回表示源代码最后一个序列的整数。</param>
    protected virtual void CaptureElementReference(RenderTreeBuilder builder, int sequence)
    {
        if (CaptureReference)
        {
            builder.AddElementReferenceCapture(sequence, element => Reference = element);
        }
    }
    #endregion

    #region AddContent
    /// <summary>
    /// 使用渲染树为组件内部添加内容。
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> 实例。</param>
    /// <param name="sequence">返回表示源代码最后一个序列的整数。</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => InvokeOnBuildContentInterceptors(builder, sequence);

    #endregion

    #region BuildComponentFeatures

    /// <inheritdoc/>
    public void BuildComponent(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
        CaptureElementReference(builder, sequence + 3);
    }

    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<string, object>> GetAttributes()
    {
        Dictionary<string, object> innerAttributes = new();

        var htmlAttributeResolvers = ServiceProvider.GetServices(typeof(IHtmlAttributeResolver)).OfType<IHtmlAttributeResolver>();

        foreach ( var resolver in htmlAttributeResolvers )
        {
            var value = resolver!.Resolve(this);
            innerAttributes.AddOrUpdateRange(value);
        }

        foreach ( var interruptor in Interceptors )
        {
            interruptor!.InterceptOnAttributesBuilding(this, innerAttributes);
        }

        // The same key of attributes from resolver can be replaced by unmatched attributes
        innerAttributes.AddOrUpdateRange(AdditionalAttributes);

        BuildAttributes(innerAttributes);

        return innerAttributes;
    }
    #endregion

    #endregion

    #endregion
}
