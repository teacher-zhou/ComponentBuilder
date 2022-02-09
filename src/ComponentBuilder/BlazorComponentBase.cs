
using ComponentBuilder.Abstrations.Internal;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

using System.Linq;

namespace ComponentBuilder;

/// <summary>
/// Provides a base class for component that can build css class quickly.
/// </summary>
public abstract partial class BlazorComponentBase : ComponentBase, IBlazorComponent, IRefreshComponent, IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of <see cref="BlazorComponentBase"/> class.
    /// </summary>
  protected BlazorComponentBase()
    {
        CssClassBuilder = ServiceProvider?.GetService<ICssClassBuilder>() ?? new DefaultCssClassBuilder();
        StyleBuilder = ServiceProvider?.GetService<IStyleBuilder>() ?? new DefaultStyleBuilder();
    }

    #region Properties

    #region Injection
    /// <summary>
    /// Injection of <see cref="IServiceProvider"/> instance.
    /// </summary>
    [Inject] private IServiceProvider ServiceProvider { get; set; }
    /// <summary>
    /// Gets injection of <see cref="IJSRuntime"/> instance.
    /// </summary>
    [Inject] protected IJSRuntime JS { get; private set; }

    #endregion Injection

    #region Parameters

    /// <summary>
    /// Gets or sets the additional attribute for element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets to append addtional CSS class.
    /// </summary>
    [Parameter] public string AdditionalCssClass { get; set; }
    /// <summary>
    /// Gets or sets to append additional style.
    /// </summary>
    [Parameter] public string AdditionalStyle { get; set; }
    /// <summary>
    /// Use <see cref="Css"/> class to invoke utility class. Make sure
    /// </summary>
    [Parameter] public ICssClassUtility CssClass { get; set; }

    #endregion Parameters

    #region Protected    
    /// <summary>
    /// Gets <see cref="ICssClassBuilder"/> instance.
    /// </summary>
    protected ICssClassBuilder CssClassBuilder { get; }
    /// <summary>
    /// Gets <see cref="IStyleBuilder"/> instance.
    /// </summary>
    protected IStyleBuilder StyleBuilder { get; }
    /// <summary>
    /// Gets html tag name to build component.
    /// </summary>
    protected virtual string TagName => ServiceProvider.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);

    /// <summary>
    /// Gets the sequence for source code from <see cref="RenderTreeBuilder"/> class of component region.
    /// </summary>
    protected virtual int RegionSequence => this.GetHashCode();
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

    #region Public

    /// <summary>
    /// Returns CSS class string for component. Overrides by 'class' attribute in element if specified.
    /// </summary>
    /// <returns>A series css class string seperated by spece for each item.</returns>
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
    /// Returns style string for component. Overrides by 'style' attribute in element if specified.
    /// </summary>
    /// <returns>A series style string seperated by ';' for each item.</returns>
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
    /// Notifies the component that its state has changed. When applicable, this will cause the component to be re-rendered. 
    /// </summary>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

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
    /// Overrides this method to build compoent render tree by yourself. Recommand override <see cref="BuildComponentRenderTree(RenderTreeBuilder)"/> method to create attributes only.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenRegion(RegionSequence);
        builder.OpenElement(0, TagName ?? "div");
        BuildComponentRenderTree(builder);
        builder.CloseElement();
        builder.CloseRegion();
    }

    /// <summary>
    /// Build component attributes by specified <see cref="RenderTreeBuilder"/> instance by configurations and resolvers.
    /// <list type="bullet">
    /// <item>
    /// Call <see cref="AddClassAttribute(RenderTreeBuilder, int)"/> method;
    /// </item>
    /// <item>
    /// Call <see cref="AddMultipleAttributes(RenderTreeBuilder, int)"/> method;
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
    /// Build component tree automatically with following steps:
    /// <list type="number">
    /// <item>
    /// Call <see cref="BuildComponentAttributes(RenderTreeBuilder, out int)"/> method to build attributes from resolvers.
    /// </item>
    /// <item>
    /// Call <see cref="AddChildContent(RenderTreeBuilder, int)"/> method to try adding child content if implemented <see cref="IHasChildContent"/>.
    /// </item>
    /// </list>
    /// <para>
    /// Ovrrides this method to build attributes, child content or events only, if you override <see cref="BuildRenderTree(RenderTreeBuilder)"/> method, you have to create element by your-own using <see cref="RenderTreeBuilder"/> class.
    /// </para>
    /// <para>
    /// Call <see cref="BuildComponentAttributes(RenderTreeBuilder, out int)"/> method manually to apply resolvers for pamaters such as <see cref="CssClassAttribute"/> class.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    protected virtual void BuildComponentRenderTree(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
    }

    /// <summary>
    /// Appends frames representing an arbitrary fragment of content.
    /// </summary>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => AddChildContent(builder, sequence);

    /// <summary>
    /// Appends frames representing an arbitrary fragment of content if component has implemeted <see cref="IHasChildContent"/>.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (this is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
    }

    /// <summary>
    /// Appends frames representing an arbitrary fragment of content if component has implemeted <see cref="IHasChildContent{TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of object.</typeparam>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="value">The value used to build the content.</param>
    protected void AddChildContent<TValue>(RenderTreeBuilder builder, int sequence, TValue value)
    {
        if (this is IHasChildContent<TValue> content)
        {
            builder.AddContent<TValue>(sequence, content.ChildContent, value);
        }
    }

    /// <summary>
    /// Append 'class' attribute to <see cref="RenderTreeBuilder"/> class that generated after <see cref="GetCssClassString"/> is called.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected virtual void AddClassAttribute(RenderTreeBuilder builder, int sequence)
    {
        var cssClass = GetCssClassString();
        if (!string.IsNullOrEmpty(cssClass))
        {
            builder.AddAttribute(sequence, "class", cssClass);
        }
    }

    /// <summary>
    /// Append 'style' attribute to <see cref="RenderTreeBuilder"/> class that generated after <see cref="GetStyleString"/> is called.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected virtual void AddStyleAttribute(RenderTreeBuilder builder, int sequence)
    {
        var style= GetStyleString();
        if (!string.IsNullOrEmpty(style))
        {
            builder.AddAttribute(sequence,"style",style);
        }
    }

    /// <summary>
    /// Adds frames representing multiple attributes with the same sequence number.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
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
            attributes = attributes.Concat(CssHelper.MergeAttributes(AdditionalAttributes));
        }

        BuildAttributes(AdditionalAttributes);
        attributes = attributes.Concat(AdditionalAttributes);

        builder.AddMultipleAttributes(sequence, attributes.Distinct());
    }

    /// <summary>
    /// Try to get 'class' attribute from element.
    /// </summary>
    /// <param name="cssClass">The value of 'class' attribute from element. It can be <c>null</c>.</param>
    /// <returns><c>true</c> for element has 'class' attribute, otherwise <c>false</c>.</returns>
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
    /// Try to get 'style' attribute from element.
    /// </summary>
    /// <param name="style">The value of 'style' attribute from element. It can be <c>null</c>.</param>
    /// <returns><c>true</c> for element has 'style' attribute, otherwise <c>false</c>.</returns>
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


    #region Dispose
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                //CssClassBuilder?.Dispose();
               // StyleBuilder?.Dispose();
                disposedValue = true;

                DisposeComponent();
            }
            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    ~BlazorComponentBase()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void DisposeComponent()
    {

    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion

    #endregion Protected

    #region Private

    #endregion

    #endregion Method
}
