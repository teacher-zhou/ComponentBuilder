using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;

using Microsoft.Extensions.DependencyInjection;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class with automation component characteristics. This is an abstract class.
/// </summary>
public abstract partial class BlazorComponentBase : ComponentBase, IBlazorComponent
{
    #region Contructor    
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorComponentBase"/> class.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected BlazorComponentBase() : base()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        ChildComponents = new();
    }
    #endregion

    #region Properties
    /// <inheritdoc/>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object?> AdditionalAttributes { get; set; } = new Dictionary<string, object?>();

    /// <inheritdoc/>
    protected ICssClassBuilder CssClassBuilder { get; private set; }
    /// <inheritdoc/>
    protected IStyleBuilder StyleBuilder { get; private set; }
    /// <summary>
    /// 获取 <see cref="IServiceProvider"/> 实例。
    /// </summary>
    [Inject][NotNull] protected IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// 获取组件拦截器集合。
    /// </summary>
    IEnumerable<IComponentInterceptor> Interceptors { get; set; }

    /// <inheritdoc/>
    public BlazorComponentCollection ChildComponents { get; private set; }

    #endregion

    #region Lifecyle

    #region Initialize    
    /// <summary>
    /// Initializes this component in <see cref="SetParametersAsync(ParameterView)"/> method.
    /// </summary>
    /// <remarks>
    /// The method provides for manually invocation when <see cref="SetParametersAsync(ParameterView)"/> is overrided.
    /// </remarks>
    protected void Initialize()
    {
        Interceptors ??= ServiceProvider!.GetServices(typeof(IComponentInterceptor)).OfType<IComponentInterceptor>().OrderBy(m => m.Order).AsEnumerable();
        CssClassBuilder = ServiceProvider!.GetRequiredService<ICssClassBuilder>();
        StyleBuilder = ServiceProvider!.GetRequiredService<IStyleBuilder>();
    }
    #endregion

    #region SetParametersAsync
    /// <summary>
    /// DO NOT OVERRIDE this method manually, override <see cref="AfterSetParameters(ParameterView)"/> instead.
    /// </summary>
    /// <param name="parameters">Received component parameters.</param>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        Initialize();
        InvokeSetParametersInterceptors(parameters);
        AfterSetParameters(parameters);

        return base.SetParametersAsync(ParameterView.Empty);
    }

    /// <summary>
    /// An interceptor method instead <see cref="SetParametersAsync(ParameterView)"/> method.
    /// </summary>
    /// <param name="parameters">Received component parameters.</param>
    protected virtual void AfterSetParameters(ParameterView parameters) { }
    #endregion

    #region OnInitialized

    /// <summary>
    /// Overrides this method to cancel interceptors.
    /// </summary>
    protected override void OnInitialized()
    {
        InvokeOnInitializeInterceptors();
        AfterOnInitialized();
    }

    /// <summary>
    /// Override this method instead <see cref="OnInitialized"/>.
    /// </summary>
    protected virtual void AfterOnInitialized() { }
    #endregion

    #region OnParameterSet
    /// <summary>
    /// Overrides this method to cancel interceptors.
    /// </summary>
    protected override void OnParametersSet()
    {
        InvokeOnParameterSetInterceptors();
        AfterOnParameterSet();
    }

    /// <summary>
    /// Override this method instead <see cref="OnParametersSet"/>.
    /// </summary>
    protected virtual void AfterOnParameterSet() { }
    #endregion

    #region OnAfterRender   
    /// <summary>
    /// Overrides this method to cancel interceptors.
    /// </summary>
    protected override void OnAfterRender(bool firstRender)
    {
        InvokeOnAfterRenderInterceptors(firstRender);
        AfterOnAfterRender(firstRender);
    }

    /// <summary>
    /// Override this method instead <see cref="OnAfterRender"/>.
    /// </summary>
    protected virtual void AfterOnAfterRender(bool firstRender) { }

    #endregion

    #endregion

    #region Interceptors

    #region InvokeSetParametersInterceptors
    /// <summary>
    /// Performs <see cref="IComponentInterceptor.InterceptOnSetParameters(IBlazorComponent, in ParameterView)"/> methods.
    /// </summary>
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
    /// Performs <see cref="IComponentInterceptor.InterceptOnInitialized(IBlazorComponent)"/> methods.
    /// </summary>
    protected void InvokeOnInitializeInterceptors()
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnInitialized(this);
        }
    }
    #endregion

    #region InvokeOnParameterSetInterceptors
    /// <summary>
    /// Performs <see cref="IComponentInterceptor.InterceptOnParameterSet(IBlazorComponent)"/> methods.
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
    /// Performs <see cref="IComponentInterceptor.InterceptOnAfterRender(IBlazorComponent, in bool)"/> methods.
    /// </summary>
    /// <param name="firstRender"><c>True</c> to indicate component is first render, otherwise, <c>false</c>.</param>
    protected void InvokeOnAfterRenderInterceptors(bool firstRender)
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnAfterRender(this, firstRender);
        }
    }
    #endregion

    #region InvokeOnBuildContentIntercepts
    /// <summary>
    /// Performs <see cref="IComponentInterceptor.InterceptOnContentBuilding(IBlazorComponent, RenderTreeBuilder, int)"/> methods.
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
    /// Performs <see cref="IComponentInterceptor.InterceptOnDisposing(IBlazorComponent)"/> methods.
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
    /// Build the CSS classes required by the component in the form of logical code.
    /// </summary>
    /// <param name="builder"><see cref="ICssClassBuilder"/> instance.</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder) { }
    #endregion

    #region BuildStyle
    /// <summary>
    /// The style required to build components as logical code.
    /// </summary>
    /// <param name="builder"><see cref="IStyleBuilder"/> instance.</param>
    protected virtual void BuildStyle(IStyleBuilder builder) { }
    #endregion

    #region BuildAttributes
    /// <summary>
    /// The HTML properties needed to build the component in the form of logical code.
    /// </summary>
    /// <param name="attributes">A component's properties include mismatched parameters that are parsed and captured from the framework.</param>
    protected virtual void BuildAttributes(IDictionary<string, object?> attributes) { }
    #endregion

    #endregion

    #region NotifyStateChanged
    /// <inheritdoc/>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
    #endregion

    #region GetCssClassString
    /// <inheritdoc/>
    internal string? GetCssClassString()
    {
        var resolvers = ServiceProvider.GetServices<IParameterClassResolver>();
        foreach (var item in resolvers)
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
    internal string? GetStyleString()
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
    /// <summary>
    /// Add a component to the current component to make it a child component.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    public virtual void AddChildComponent(IBlazorComponent component)
    {
        ArgumentNullException.ThrowIfNull(component);

        ChildComponents.Add(component);
        StateHasChanged();

    }
    #endregion

    #region Dispose
    private bool _disposedValue;

    /// <summary>
    /// Release component resources.
    /// </summary>
    protected virtual void DisposeComponentResources()
    {

    }

    /// <summary>
    /// Release the managed resources.
    /// </summary>
    protected virtual void DisposeManagedResouces()
    {

    }

    /// <summary>
    /// Performs component resource release.
    /// </summary>
    /// <param name="disposing"><c>true</c> if you want to release managed resources.</param>
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
    /// Destructor
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
    /// Gets or sets a Boolean value indicating whether a reference to an HTML element is captured.
    /// </summary>
    protected bool CaptureReference { get; set; }

    /// <summary>
    /// When <see cref="CaptureReference"/> is <c>true</c>, a reference to the HTML element is obtained.
    /// </summary>
    protected ElementReference? Reference { get; private set; }
    #endregion

    #region Method

    #region GetRegionSequence
    /// <summary>
    /// Overwrites the source code sequence that generates the RenderTreeBuilder startup sequence.
    /// </summary>
    protected virtual int GetRegionSequence() => new Random().Next(100, 999);
    #endregion

    #region GetTagName

    /// <inheritdoc/>
    protected internal virtual string GetTagName()
        => ServiceProvider!.GetRequiredService<IHtmlTagAttributeResolver>()!.Resolve(this);
    #endregion

    #region BuildRenderTree
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="builder"></param>
    /// <exception cref="InvalidOperationException">No any component renderer.</exception>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var renderers = ServiceProvider.GetServices<IComponentRenderer>().OfType<IComponentRenderer>();

        if (!renderers.Any())
        {
            throw new InvalidOperationException("No renderers found, at least one must be provided");
        }

        foreach (var item in renderers)
        {
            if (!item.Render(this, builder))
            {
                break;
            }
        }
    }
    #endregion

    #region BuildComponentAttributes
    /// <summary>
    /// Builds HTML and component properties for the specified render tree.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> instance.</param>
    /// <param name="sequence">Returns an integer representing the last sequence of the source code.</param>
    protected void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence) => builder.AddMultipleAttributes(sequence = 4, GetAttributes()!);
    #endregion

    #region CaptureElementReference
    /// <summary>
    /// When <see cref="CaptureReference"/> is <c>true</c>, a Reference to the element is automatically captured and assigned to <see cref=" Reference"/> after the component is rendered.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> instance.</param>
    /// <param name="sequence">Returns an integer representing the last sequence of the source code.</param>
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
    /// Use the render tree to add content inside a component.
    /// </summary>
    /// <param name="builder"><see cref="RenderTreeBuilder"/> instance.</param>
    /// <param name="sequence">Returns an integer representing the last sequence of the source code.</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => InvokeOnBuildContentInterceptors(builder, sequence);

    #endregion

    #region BuildComponentFeatures

    /// <inheritdoc/>
    protected internal void BuildComponent(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
        CaptureElementReference(builder, sequence + 3);
    }

    /// <inheritdoc/>
    public IEnumerable<KeyValuePair<string, object?>> GetAttributes()
    {
        Dictionary<string, object?> innerAttributes = [];

        var htmlAttributeResolvers = ServiceProvider.GetServices(typeof(IHtmlAttributeResolver)).OfType<IHtmlAttributeResolver>();

        foreach (var resolver in htmlAttributeResolvers)
        {
            var value = resolver!.Resolve(this);
            innerAttributes.AddOrUpdateRange(value);
        }

        foreach (var interruptor in Interceptors)
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
