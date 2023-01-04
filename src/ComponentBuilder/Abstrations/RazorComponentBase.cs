using Microsoft.Extensions.DependencyInjection;
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
public abstract partial class RazorComponentBase : ComponentBase,IComponent, IHasAdditionalAttributes, IRefreshableComponent, IDisposable
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
    [Inject][NotNull] protected ICssClassBuilder CssClassBuilder { get; set; }
    /// <summary>
    /// Gets <see cref="IStyleBuilder"/> instance.
    /// </summary>
    [Inject][NotNull] protected IStyleBuilder StyleBuilder { get; set; }
    /// <summary>
    /// Gets <see cref="IServiceProvider"/> instance.
    /// </summary>
    [Inject][NotNull] protected IServiceProvider ServiceProvider { get; set; }

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


    #region Can Override

    /// <summary>
    /// <inheritdoc/> 
    /// <para>
    /// Overrides but manually call <see cref="OnComponentInitialized"/> for cascading parent component feature.
    /// </para>
    /// </summary>
    protected override void OnInitialized()
    {
        OnComponentInitialized();
        base.OnInitialized();
    }

    /// <summary>
    /// Method invoked when componen is ready to start, 
    /// and automatically to create cascading component that defined <see cref="ParentComponentAttribute"/> class.
    /// </summary>
    protected virtual void OnComponentInitialized()
    {
        CssClassBuilder.Dispose();
        AssociateChildComponent();
    }


    /// <summary>
    /// Method invoked when the component has received parameters from its parent in
    /// the render tree, and the incoming values have been assigned to properties.
    /// 
    /// <para>
    /// Overrides but manually call <see cref="OnComponentParameterSet"/> for HTML attributes resolving feature.
    /// </para>
    /// </summary>
    protected override void OnParametersSet()
    {
        OnComponentParameterSet();
        base.OnParametersSet();
    }
    /// <summary>
    /// Method invoke to resolve attributes automatically for parameters.
    /// </summary>
    protected virtual void OnComponentParameterSet() => ResolveHtmlAttributes();

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

    /// <summary>
    /// Resolve HTML attributes.
    /// </summary>
    protected void ResolveHtmlAttributes()
    {
        Dictionary<string, object>? htmlAttributes = new();

        htmlAttributes.AddRange(AdditionalAttributes);

        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributesResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            var value = resolver.Resolve(this);
            htmlAttributes.AddRange(value);
        }

        BuildAttributes(htmlAttributes!);

        //TODO Modify to pipeline pattern

        BuildClass(htmlAttributes!);
        BuildStyle(htmlAttributes!);

        AdditionalAttributes = htmlAttributes;

        #region Local Function
        void BuildClass(Dictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("class"))
            {
                var result = ServiceProvider.GetRequiredService<ICssClassAttributeResolver>()!.Resolve(this);
                CssClassBuilder.Append(result);

                BuildCssClass(CssClassBuilder);

                if (this is IHasCssClassUtility cssClassUtility)
                {
                    CssClassBuilder.Append(cssClassUtility?.CssClass?.CssClasses ?? Enumerable.Empty<string>());
                }

                if (this is IHasAdditionalClass additionalCssClass)
                {
                    CssClassBuilder.Append(additionalCssClass.AdditionalClass);
                }
                var css = CssClassBuilder.ToString();
                if (!string.IsNullOrEmpty(css))
                {
                    htmlAttributes.Add("class", css);
                }
            }
        }

        void BuildStyle(Dictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("style"))
            {
                StyleBuilder.Dispose();
                this.BuildStyle(StyleBuilder);

                if (this is IHasAdditionalStyle additionalStyle)
                {
                    StyleBuilder.Append(additionalStyle.AdditionalStyle);
                }

                var style = StyleBuilder.ToString();
                if (!string.IsNullOrEmpty(style))
                {
                    htmlAttributes.Add("style", style);
                }
            }
        }
        #endregion
    }

    #region ChildComponent
    /// <summary>
    /// Associate with specified parent component witch has defined <see cref="ChildComponentAttribute"/> attribute and throw exception if <see cref="ChildComponentAttribute.Optional"/> is not <c>true</c>.
    /// </summary>
    protected void AssociateChildComponent()
    {
        var componentType = GetType();

        //TODO Replace Nullable to associate with parent component for Optional

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
                    throw new InvalidOperationException(@$"
Component {componentType.Name} has defined {nameof(ChildComponentAttribute)} attribute, it means this component can only be the child of {attr.ComponentType.Name} component, like:

<{attr.ComponentType.Name}>
    <{componentType.Name}></{componentType.Name}>
    ...
    <{componentType.Name}></{componentType.Name}>
</{attr.ComponentType.Name}>

Then you can have a cascading parameter of {attr.ComponentType.Name} component with public modifier get the instance automatically, like: 

[CascadingParameter]public {attr.ComponentType.Name}? MyParent {{ get; set; }}

Set Optional is true of {nameof(ChildComponentAttribute)} can ignore this exception means current component can be child component of {attr.ComponentType.Name} optionally, and the cascading parameter of parent component may be null.
");
                }

                if (propertyType is not null && propertyValue is not null)
                {
                    propertyType!.GetMethod(nameof(AddChildComponent))!
                        .Invoke(propertyValue!, new[] { this });
                }
            }
        }
    }

    /// <summary>
    /// Add a component to current component be child component.
    /// </summary>
    /// <param name="component">A component to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    public virtual void AddChildComponent(IComponent component)
    {
        if (component is null)
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
    protected virtual void DisposeComponent()
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

            DisposeComponent();

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
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}
