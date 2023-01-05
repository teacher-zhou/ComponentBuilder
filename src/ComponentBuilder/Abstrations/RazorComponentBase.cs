using ComponentBuilder.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class with automation component features. This is an abstract class.
/// <para>
/// The class only for .razor file component to inherit.
/// </para>
/// </summary>
public abstract partial class RazorComponentBase : ComponentBase,IComponent, IHasAdditionalAttributes, IRazorComponent, IDisposable
{
    #region Contructor
    /// <summary>
    /// Initializes a new instance of the <see cref="RazorComponentBase"/> class.
    /// </summary>
    public RazorComponentBase()
    {
        AdditionalAttributes ??= new Dictionary<string, object>();
    }


    #endregion

    /// <inheritdoc/>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

    #region Propertis
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
    [Inject][NotNull] protected IServiceProvider ServiceProvider { get; set; }

    [Inject] IOptions<ComponentBuilderOptions> ComponentBuilderOptions { get; set; }

    IEnumerable<IComponentInterceptor>? Interceptors { get; set; }

    /// <summary>
    /// Gets the options configure in services.
    /// </summary>
    protected internal ComponentBuilderOptions Options => ComponentBuilderOptions.Value;

    /// <summary>
    /// Gets a collection of child components that associated with current component.
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    /// <summary>
    /// Get <see cref="IJSRuntime"/> instance after lazy initialized.
    /// </summary>
    protected Lazy<IJSRuntime> JS
    {
        get
        {
            var js = ServiceProvider?.GetService<IJSRuntime>();
            if ( js is not null )
            {
                return new(() => js, LazyThreadSafetyMode.PublicationOnly);
            }
            return new Lazy<IJSRuntime>();
        }
    }
    #endregion

    #region Lifecyle

    private void InitializeInjections()
    {
        Interceptors = ServiceProvider!.GetServices<IComponentInterceptor>();
        CssClassBuilder = ServiceProvider!.GetRequiredService<ICssClassBuilder>();
        StyleBuilder = ServiceProvider!.GetRequiredService<IStyleBuilder>();
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);

        InvokeSetParametersInterceptors(parameters);

        return base.SetParametersAsync(ParameterView.Empty);
    }

    protected override void OnInitialized()
    {
        InvokeOnInitializeInterceptors();
    }

    protected override void OnParametersSet()
    {
        InvokeOnParameterSetInterceptors();

    }

    protected override void OnAfterRender(bool firstRender) => InvokeOnAfterRenderInterceptors(firstRender);
    #endregion

    #region Interceptors

    protected void InvokeSetParametersInterceptors(ParameterView parameters)
    {
        InitializeInjections();

        if ( Interceptors is null )
        {
            return;
        }
        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnSetParameters(this, parameters);
        }
    }

    protected void InvokeOnInitializeInterceptors()
    {
        if(Interceptors is null )
        {
            return;
        }

        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnInitialized(this);
        }
    }
    protected void InvokeOnParameterSetInterceptors()
    {
        ResolveHtmlAttributes();

        if ( Interceptors is null )
        {
            return;
        }
        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnParameterSet(this);
        }
    }

    protected void InvokeOnAfterRenderInterceptors(bool firstRender)
    {
        if ( Interceptors is null )
        {
            return;
        }

        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnAfterRender(this, firstRender);
        }
    }

    protected void InvokeOnDispose()
    {
        if ( Interceptors is null )
        {
            return;
        }

        foreach ( var interruptor in Interceptors )
        {
            interruptor.InterceptOnDispose(this);
        }
    }
    #endregion

    #region Build Class/Style/Attributes
    /// <summary>
    /// Overrides to build CSS class for component customization.
    /// <para>
    /// You can build CSS class of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="builder">A <see cref="ICssClassBuilder"/> instance.</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder) { }

    /// <summary>
    /// Overrides to build style for component customization.
    /// <para>
    /// You can build style of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="builder">A <see cref="IStyleBuilder"/> instance.</param>
    protected virtual void BuildStyle(IStyleBuilder builder) { }


    /// <summary>
    /// Overrides to build attributes for component customization.
    /// <para>
    /// You can build attributes of component with parameters for own logical code.
    /// </para>
    /// </summary>
    /// <param name="attributes">The attributes for components.</param>
    protected virtual void BuildAttributes(IDictionary<string, object> attributes) { }
    #endregion

    /// <inheritdoc/>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

    public string? GetCssClassString()
    {
        var result = ServiceProvider.GetRequiredService<ICssClassAttributeResolver>()!.Resolve(this);
        CssClassBuilder.Append(result);

        BuildCssClass(CssClassBuilder);

        if ( this is IHasCssClassUtility cssClassUtility )
        {
            CssClassBuilder.Append(cssClassUtility?.CssClass?.CssClasses ?? Enumerable.Empty<string>());
        }

        if ( this is IHasAdditionalClass additionalCssClass )
        {
            CssClassBuilder.Append(additionalCssClass.AdditionalClass);
        }
        return CssClassBuilder.ToString();
    }

    public string? GetStyleString()
    {
        this.BuildStyle(StyleBuilder);

        if ( this is IHasAdditionalStyle additionalStyle )
        {
            StyleBuilder.Append(additionalStyle.AdditionalStyle);
        }

        return StyleBuilder.ToString();
    }

    /// <summary>
    /// Resolve HTML attributes.
    /// </summary>
    protected void ResolveHtmlAttributes()
    {
        Dictionary<string, object>? innerAttributes = new();
        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributeResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            var value = resolver.Resolve(this);
            innerAttributes.UpdateRange(value);
        }

        if ( Interceptors is not null )
        {
            foreach ( var interruptor in Interceptors )
            {
                interruptor.InterceptOnResolvedAttributes(this, innerAttributes);
            }
        }

        BuildAttributes(innerAttributes);

        // the outer attributes set by user should replace the inner attribute with same key
        AdditionalAttributes.UpdateRange(innerAttributes, false);
    }

    #region ChildComponent
    /// <summary>
    /// Add a component to current component be child component.
    /// </summary>
    /// <param name="component">A component to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    public void AddChildComponent(IRazorComponent component)
    {
        if ( component is null )
        {
            throw new ArgumentNullException(nameof(component));
        }

        ChildComponents.Add(component);
        StateHasChanged();
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

            CssClassBuilder.Clear();
            StyleBuilder.Clear();

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Finalizes the resouces.
    /// </summary>
    ~RazorComponentBase()
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
}
