
using ComponentBuilder.Abstrations.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

using System.Linq;

namespace ComponentBuilder;

/// <summary>
/// Provides a base class for component that can build css class quickly.
/// </summary>
public abstract partial class BlazorComponentBase : ComponentBase, IBlazorComponent, IDisposable
{
    private bool disposedValue;
    #region Properties

    #region Injection
    /// <summary>
    /// Gets injection of <see cref="ICssClassBuilder"/> instance.
    /// </summary>
    [Inject] protected ICssClassBuilder CssClassBuilder { get; set; }
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
    /// Gets or sets to append addtional css class.
    /// </summary>
    [Parameter] public string AdditionalCssClass { get; set; }
    /// <summary>
    /// Use <see cref="Css"/> class to invoke utility class. Make sure
    /// </summary>
    [Parameter] public ICssClassUtility CssClass { get; set; }

    #endregion Parameters

    #region Protected    
    /// <summary>
    /// Gets html tag name to build component.
    /// </summary>
    protected virtual string TagName => ServiceProvider.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);

    /// <summary>
    /// Gets the sequence for source code from <see cref="RenderTreeBuilder"/> class of component region.
    /// </summary>
    protected virtual int RegionSequence => new Random().Next();
    #endregion

    #endregion Properties

    #region Method

    #region Public

    /// <summary>
    /// Returns css class string for component. Overrides by 'class' attribute in element specified.
    /// </summary>
    /// <returns>A series css class string seperated by spece for each item.</returns>
    public virtual string? GetCssClassString()
    {
        if (TryGetClassAttribute(out var value))
        {
            return value;
        }

        CssClassBuilder.Append(ServiceProvider.GetService<ICssClassAttributeResolver>()?.Resolve(this));

        BuildCssClass(CssClassBuilder);

        if (CssClass is not null)
        {
            CssClassBuilder.Append(CssClass.CssClasses);
        }

        if (!string.IsNullOrEmpty(AdditionalCssClass))
        {
            CssClassBuilder.Append(AdditionalCssClass);
        }

        return CssClassBuilder.ToString();
    }

    /// <summary>
    /// Notifies the component that its state has changed. When applicable, this will cause the component to be re-rendered. 
    /// </summary>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

    #endregion Public

    #region Protected

    #region Can Override
    /// <summary>
    /// Overrides to build css class by special logical process.
    /// </summary>
    /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
    protected virtual void BuildCssClass(ICssClassBuilder builder)
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
    /// </summary>
    /// <param name="builder">A <see cref="RenderTreeBuilder"/> to create component.</param>
    /// <param name="sequence">An integer that represents the last position of the instruction in the source code.</param>
    protected virtual void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        AddClassAttribute(builder, 1);
        AddMultipleAttributes(builder, sequence = 2);
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
        sequence = AddEventCallbacks(builder, sequence + 1);
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
    /// Try to appends frames representing an attribute for event callbacks.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <returns>The last position of source code.</returns>
    protected int AddEventCallbacks(RenderTreeBuilder builder, int sequence)
    {
        var eventCallbacks = ServiceProvider.GetService<IHtmlEventAttributeResolver>()?.Resolve(this);
        foreach (var callback in eventCallbacks)
        {
            builder.AddAttribute(sequence, callback.Key, callback.Value);
            sequence++;
        }
        return sequence;
    }

    /// <summary>
    /// Append 'class' attribute to <see cref="RenderTreeBuilder"/> class that generated after <see cref="GetCssClassString"/> called..
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
    /// Adds frames representing multiple attributes with the same sequence number.
    /// </summary>
    /// <param name="builder">The <see cref="RenderTreeBuilder"/> class to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected void AddMultipleAttributes(RenderTreeBuilder builder, int sequence)
    {

        var attributes = new Dictionary<string, object>().AsEnumerable();

        if (AdditionalAttributes is not null)
        {
            attributes = CssHelper.MergeAttributes(AdditionalAttributes);
        }

        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributesResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            attributes = attributes.Concat(resolver.Resolve(this));
        }

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

    #region Dispose
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            CssClassBuilder?.Dispose();
            disposedValue = true;
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion

    #endregion Protected

    #region Private

    #endregion

    #endregion Method
}
