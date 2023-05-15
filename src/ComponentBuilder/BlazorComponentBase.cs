using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class with automation component features. This is an abstract class.
/// </summary>
public abstract partial class BlazorComponentBase : ComponentBase, IBlazorComponent
{
    #region Contructor
    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorComponentBase"/> class.
    /// </summary>
    protected BlazorComponentBase() : base()
    {
        AdditionalAttributes = new Dictionary<string, object>();
    }
    #endregion

    #region Properties
    /// <inheritdoc/>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets <see cref="ICssClassBuilder"/> instance.
    /// </summary>
    public ICssClassBuilder CssClassBuilder { get; private set; }
    /// <summary>
    /// Gets <see cref="IStyleBuilder"/> instance.
    /// </summary>
    public IStyleBuilder StyleBuilder { get; private set; }
    /// <summary>
    /// Gets <see cref="IServiceProvider"/> instance.
    /// </summary>
    [Inject][NotNull] protected IServiceProvider? ServiceProvider { get; set; }

    /// <summary>
    /// Gets the injection instance of  <see cref="IOptions{TOptions}"/>.
    /// </summary>
    [Inject] IOptions<ComponentBuilderOptions> ComponentBuilderOptions { get; set; }

    /// <summary>
    /// Gets the list of interceptors.
    /// </summary>
    IEnumerable<IComponentInterceptor> Interceptors { get; set; }

    /// <summary>
    /// Gets the options configure in services.
    /// </summary>
    protected internal ComponentBuilderOptions Options => ComponentBuilderOptions.Value;

    /// <summary>
    /// Gets a collection of child components that associated with current component.
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    #region JS
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
    #endregion

    #endregion

    #region Lifecyle

    #region Initialize
    /// <summary>
    /// Initialize the services for component. It must be called in <see cref="SetParametersAsync(ParameterView)"/> method.
    /// </summary>
    protected void Initialize()
    {
        Interceptors ??= ServiceProvider!.GetServices(typeof(IComponentInterceptor)).OfType<IComponentInterceptor>().OrderBy(m => m.Order).AsEnumerable();
        CssClassBuilder ??= ServiceProvider?.GetService<ICssClassBuilder>() ?? new DefaultCssClassBuilder();
        StyleBuilder ??= ServiceProvider?.GetService<IStyleBuilder>() ?? new DefaultStyleBuilder();
    }
    #endregion

    #region SetParametersAsync
    /// <summary>
    /// Sets parameters supplied by the component's parent in the render tree.
    /// </summary>
    /// <param name="parameters">The parameters.</param>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that completes when the component has finished updating and rendering itself.</returns>
    /// <remarks><para>
    /// Parameters are passed when <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.SetParametersAsync(Microsoft.AspNetCore.Components.ParameterView)" /> is called. It is not required that
    /// the caller supply a parameter value for all of the parameters that are logically understood by the component.
    /// </para><para>
    /// The default implementation of <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.SetParametersAsync(Microsoft.AspNetCore.Components.ParameterView)" /> will set the value of each property
    /// decorated with <see cref="T:Microsoft.AspNetCore.Components.ParameterAttribute" /> or <see cref="T:Microsoft.AspNetCore.Components.CascadingParameterAttribute" /> that has
    /// a corresponding value in the <see cref="T:Microsoft.AspNetCore.Components.ParameterView" />. Parameters that do not have a corresponding value
    /// will be unchanged.
    /// </para>
    /// <remarks>
    /// NOTE: After overriding must be call <see cref="InvokeSetParametersInterceptors(ParameterView)"/> method manully after <see cref="ParameterView.SetParameterProperties(object)"/> is called, or you will be lost automation features.
    /// </remarks>
    /// </remarks>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        Initialize();
        InvokeSetParametersInterceptors(parameters);
        AfterSetParameters(parameters);

        return base.SetParametersAsync(ParameterView.Empty);
    }

    /// <summary>
    /// Provides a method after component has set parameters.
    /// </summary>
    /// <param name="parameters">The parameters of component received.</param>
    protected virtual void AfterSetParameters(ParameterView parameters) { }
    #endregion

    #region OnInitialized
    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// </summary>
    /// <remarks>
    /// NOTE: After overriding must be call <see cref="InvokeOnInitializeInterceptors()"/> method manully, or you will be lost automation features.
    /// </remarks>
    protected override void OnInitialized()
    => InvokeOnInitializeInterceptors();
    #endregion

    #region OnParameterSet
    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// </summary>
    /// <remarks>
    /// NOTE: After overriding must be call <see cref="InvokeOnParameterSetInterceptors()"/> method manully, or you will be lost automation features.
    /// </remarks>
    protected override void OnParametersSet() => InvokeOnParameterSetInterceptors();
    #endregion

    #region OnAfterRender        
    /// <summary>
    /// Method invoked after each time the component has been rendered.
    /// </summary>
    /// <param name="firstRender">
    /// Set to <c>true</c> if this is the first time <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> has been invoked
    /// on this component instance; otherwise <c>false</c>.
    /// </param>
    /// <remarks>
    /// The <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRender(System.Boolean)" /> and <see cref="M:Microsoft.AspNetCore.Components.ComponentBase.OnAfterRenderAsync(System.Boolean)" /> lifecycle methods
    /// are useful for performing interop, or interacting with values received from <c>@ref</c>.
    /// Use the <paramref name="firstRender" /> parameter to ensure that initialization work is only performed
    /// once.
    ///  <para>
    /// NOTE: After overriding must be call <see cref="InvokeOnAfterRenderInterceptors(bool)"/> method manully, or you will be lost automation features.
    /// </para>
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
    /// It must be called in <see cref="SetParametersAsync(ParameterView)"/> method manually when <see cref="SetParametersAsync(ParameterView)"/> is overrided.
    /// </summary>
    /// <param name="parameters">The parameters of component received.</param>
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
    /// It must be called in <see cref="OnInitialized"/> method manually when <see cref="OnInitialized"/> is overrided.
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
    /// It must be called in <see cref="OnParametersSet"/> method manually when <see cref="OnParametersSet"/> is overrided.
    /// </summary>
    protected void InvokeOnParameterSetInterceptors()
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnParameterSet(this);
        }

        //BuildAttributes(AdditionalAttributes);
    }
    #endregion

    #region InvokeOnAfterRenderInterceptors
    /// <summary>
    /// It must be called in <see cref="OnAfterRender(bool)"/> method manually when <see cref="OnAfterRender(bool)"/> is overrided.
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

    /// <summary>
    /// It should be called in <see cref="AddContent(RenderTreeBuilder, int)"/> method manually when <see cref="AddContent(RenderTreeBuilder, int)"/> is overrided.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="sequence"></param>
    void InvokeOnBuildContentIntercepts(RenderTreeBuilder builder, int sequence)
    {
        foreach (var interruptor in Interceptors)
        {
            interruptor.InterceptOnContentBuilding(this, builder, sequence);
        }
    }

    #region InvokeOnDispose
    /// <summary>
    /// Call in <see cref="Dispose(bool)"/> method.
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
    /// Overrides to build CSS class for component customization.
    /// <para>
    /// You can build CSS class of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="builder">A <see cref="ICssClassBuilder"/> instance.</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder) { }
    #endregion

    #region BuildStyle
    /// <summary>
    /// Overrides to build style for component customization.
    /// <para>
    /// You can build style of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="builder">A <see cref="IStyleBuilder"/> instance.</param>
    protected virtual void BuildStyle(IStyleBuilder builder) { }
    #endregion

    #region BuildAttributes
    /// <summary>
    /// Overrides to build attributes for component customization.
    /// <para>
    /// You can build attributes of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="attributes">The attributes for components including resolved from framework and unmatched parameters that captured.</param>
    protected virtual void BuildAttributes(IDictionary<string, object> attributes) { }
    #endregion

    #endregion

    #region NotifyStateChanged
    /// <inheritdoc/>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);
    #endregion

    #region GetCssClassString
    /// <summary>
    /// Gets a string created by <see cref="CssClassBuilder"/> instance.
    /// <list type="number">
    /// <item>
    /// Resolve <see cref="ICssClassResolver"/> instance for the object that defined <see cref="CssClassAttribute"/>.
    /// </item>
    /// <item>
    /// The <see cref="BuildCssClass(ICssClassBuilder)"/> method will be called.
    /// </item>
    /// <item>
    /// The <see cref="IHasCssClassUtility.CssClass"/> will be appended if implemented.
    /// </item>
    /// <item>
    /// The <see cref="IHasAdditionalClass.AdditionalClass"/> will be appended if implemented.
    /// </item>
    /// </list>
    /// </summary>
    /// <returns>A string seperated by space for each item or <c>null</c>. </returns>
    public string? GetCssClassString()
    {
        var resolvers = ServiceProvider.GetServices<ICssClassResolver>();
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
    /// <summary>
    /// Gets a string created by <see cref="StyleBuilder"/> instance.
    /// <list type="number">
    /// <item>
    /// The <see cref="BuildStyle(IStyleBuilder)"/> method will be called
    /// </item>
    /// <item>
    /// The <see cref="IHasAdditionalStyle.AdditionalStyle"/> will be appended if implemented.
    /// </item>
    /// </list>
    /// </summary>
    /// <returns>A string seperated by semi-colon(;) for each item or <c>null</c>. </returns>
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
    /// Notifies this component should re-render after child components had been added.
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
    /// Add a component to current component to be the child component if not exist.
    /// </summary>
    /// <param name="component">A component to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
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
    /// Dispose component resouces.
    /// </summary>
    protected virtual void DisposeComponentResources()
    {

    }

    /// <summary>
    /// Dispose managed objects of state.
    /// </summary>
    protected virtual void DisposeManagedResouces()
    {

    }

    /// <summary>
    /// Disposes the resouces of component.
    /// </summary>
    /// <param name="disposing"><c>True</c> to dispose managed resouces.</param>
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
    /// Finalizes the resouces.
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
    /// Gets or sets a boolean value wheither to capture the reference of html element.
    /// </summary>
    protected bool CaptureReference { get; set; }

    /// <summary>
    /// Gets the reference of element when <see cref="CaptureReference"/> is <c>true</c>.
    /// </summary>
    public ElementReference? Reference { get; protected set; }
    #endregion

    #region Method

    #region GetRegionSequence
    /// <summary>
    /// Overrides the source code sequence that generates the startup sequence of RenderTreeBuilder.
    /// </summary>
    protected virtual int GetRegionSequence() => GetHashCode();
    #endregion

    #region GetTagName
    /// <summary>
    /// Returns the HTML element tag name. Default to get <see cref="HtmlTagAttribute"/> defined by component class.
    /// </summary>
    /// <returns>The element tag name to create HTML element.</returns>
    public virtual string GetTagName()
        => ServiceProvider?.GetService<HtmlTagAttributeResolver>()?.Resolve(this) ?? "div";
    #endregion

    #region BuildRenderTree
    /// <summary>
    /// Automatically build component by ComponentBuilder with new region. 
    /// <para>
    /// NOTE: Override to build component by yourself, and remember call <see cref="BuildComponent(RenderTreeBuilder)"/> to apply automatic features for specific <see cref="RenderTreeBuilder"/> instance.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> .</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        var renderers = ServiceProvider.GetServices<IComponentRender>().OfType<IComponentRender>();

        if ( !renderers.Any() )
        {
            throw new InvalidOperationException("No renderers found, please provide at least one");
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
    /// Build component attributes to supplies <see cref="RenderTreeBuilder"/> instance.
    /// <para>
    /// <note type="important">
    /// NOTE: Overrides may lose all features of ComponentBuilder framework.
    /// </note>
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">Return an integer number representing the last sequence of source code.</param>
    protected void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        builder.AddMultipleAttributes(sequence = 4, GetAttributes());
    }
    #endregion

    #region CaptureElementReference
    /// <summary>
    /// Capture the element reference if <see cref="CaptureReference"/> is <c>true</c>.
    /// <para>
    /// <see cref="Reference"/> will be null if this method is never called.
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">Return an integer number representing the last sequence of source code.</param>
    protected virtual void CaptureElementReference(RenderTreeBuilder builder, int sequence)
    {
        if (Options.CaptureReference || CaptureReference)
        {
            builder.AddElementReferenceCapture(sequence, element => Reference = element);
        }
    }
    #endregion

    #region AddContent
    /// <summary>
    /// Add innter content to component.
    /// <para>
    /// Implement from <see cref="IHasChildContent"/> interface to add ChildContent automatically.
    /// </para>
    /// <para>
    /// Normally, override this method to build inner html structures.
    /// </para>
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => InvokeOnBuildContentIntercepts(builder, sequence);

    #endregion

    #region BuildComponentFeatures
    /// <summary>
    /// Build the features of element or component by ComponentBuilder Framework.
    /// <para>
    /// You can build features for any specific <see cref="RenderTreeBuilder"/> instance.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to apply the features.</param>
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
