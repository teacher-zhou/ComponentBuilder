using System.Diagnostics;
using System.Reflection;

using ComponentBuilder.Abstrations.Internal;

using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace ComponentBuilder;

/// <summary>
/// Represents a base class with automation component features. This is an abstract class.
/// </summary>
public abstract class BlazorComponentBase : ComponentBase, IComponent, IRefreshableComponent, IDisposable
{
    private bool disposedValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BlazorComponentBase"/> class.
    /// </summary>
    protected BlazorComponentBase()
    {
        CssClassBuilder = ServiceProvider?.GetService<ICssClassBuilder>() ?? new DefaultCssClassBuilder();
        StyleBuilder = ServiceProvider?.GetService<IStyleBuilder>() ?? new DefaultStyleBuilder();

        if (this is IHasForm)
        {
            _handleSubmitDelegate = SubmitFormAsync;
        }
    }

    #region Properties

    #region Injection
    /// <summary>
    /// Gets <see cref="IServiceProvider"/> instance.
    /// </summary>
    [Inject] protected IServiceProvider ServiceProvider { get; set; }

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

    [Obsolete("The method will be removed in next version, please change to GetElementTagName() method.")]
    protected virtual string? TagName
    {
        get
        {
            var tagName = ServiceProvider?.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);
            return tagName;
        }
    }

    /// <summary>
    /// Returns the HTML element tag name. Default to get <see cref="HtmlTagAttribute"/> defined by component class.
    /// </summary>
    /// <returns>The element tag name to create HTML element.</returns>
    protected virtual string? GetElementTagName()
    {
        var tagName = ServiceProvider?.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);
        return tagName;
    }

    /// <summary>
    /// Overrides the source code sequence that generates the startup sequence in the instance.
    /// </summary>
    protected virtual int RegionSequence => GetHashCode();

    /// <summary>
    /// Gets a collection of child components. 
    /// Nomally, this collection is not empty when component that define <see cref="ParentComponentAttribute"/> and child components are using into this component.
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    /// <summary>
    /// Gets or sets the action to take when a child component is added.
    /// </summary>
    protected Action<IComponent>? OnComponentAdded { get; set; }

    /// <summary>
    /// Gets or sets a boolean value wheither to capture the reference of html element.
    /// </summary>
    protected bool CaptureReference { get; set; }

    /// <summary>
    /// Gets the reference of element when <see cref="CaptureReference"/> is <c>true</c>.
    /// </summary>
    protected ElementReference? Reference { get; private set; }
    #endregion

    #endregion Properties

    #region Method

    #region Core
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
        AddCascadingComponent();

        if (this is IHasNavLink navLink)
        {
            navLink.NavigationManager.LocationChanged += OnNavLinkLocationChanged;
        }
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
    protected virtual void OnComponentParameterSet()
    {
        OnFormParameterSet();
        ResolveHtmlAttributes();
    }

    /// <summary>
    /// Automatically build component by ComponentBuilder with new region. 
    /// <para>
    /// NOTE: Override to build component by yourself, and remember call <see cref="BuildComponentFeatures(RenderTreeBuilder)"/> to apply automatic features for specific <see cref="RenderTreeBuilder"/> instance.
    /// </para>
    /// </summary>
    /// 
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> .</param>
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenRegion(RegionSequence);
        CreateComponentTree(builder, BuildComponentFeatures);
        builder.CloseRegion();
    }

    #endregion

    #region Public

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
        return ((IRefreshableComponent)this).Refresh();
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
    protected virtual void BuildComponentAttributes(RenderTreeBuilder builder, out int sequence)
    {
        if (CaptureReference)
        {
            builder.AddElementReferenceCapture(3, element => Reference = element);
        }
        AddMultipleAttributes(builder, sequence = 4);

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
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence)
        => AddChildContent(builder, sequence);

    /// <summary>
    /// Add ChildContent parameter to this component if <see cref="IHasChildContent"/> is implemented.
    /// </summary>
    /// <param name="builder">A instance of <see cref="RenderTreeBuilder"/> .</param>
    /// <param name="sequence">An integer number representing the sequence of source code.</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (this is IHasForm form)
        {
            builder.CreateCascadingComponent(_fixedEditContext, 0, content =>
            {
                content.AddContent(0, form.ChildContent?.Invoke(_fixedEditContext));
            }, isFixed: true);
        }
        else if (this is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
    }

    #endregion


    /// <summary>
    /// Build the features of element or component by ComponentBuilder Framework.
    /// <para>
    /// You can build features for any specific <see cref="RenderTreeBuilder"/> instance.
    /// </para>
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to apply the features.</param>
    protected void BuildComponentFeatures(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
    }

    /// <summary>
    /// Resolve HTML attributes.
    /// </summary>
    protected void ResolveHtmlAttributes()
    {
        var capturedAttributes = new Dictionary<string, object>().AsEnumerable();

        var htmlAttributeResolvers = ServiceProvider.GetServices<IHtmlAttributesResolver>();
        foreach (var resolver in htmlAttributeResolvers)
        {
            var value = resolver.Resolve(this);
            capturedAttributes = capturedAttributes.Merge(value);
        }

        var eventCallbacks = ServiceProvider.GetService<IHtmlEventAttributeResolver>()?.Resolve(this);

        if (eventCallbacks is not null)
        {
            capturedAttributes = capturedAttributes.Merge(eventCallbacks);
        }

        capturedAttributes = capturedAttributes.Merge(AdditionalAttributes);

        var htmlAttributes = new Dictionary<string, object>(AdditionalAttributes.Merge(capturedAttributes));

        BuildAttributes(htmlAttributes);


        BuildNavLink(htmlAttributes);
        BuildForm(htmlAttributes);

        BuildClass(htmlAttributes);
        BuildStyle(htmlAttributes);



        AdditionalAttributes = htmlAttributes;

        #region Local Function
        void BuildClass(Dictionary<string, object> htmlAttributes)
        {
            if (!htmlAttributes.ContainsKey("class"))
            {
                var result = ServiceProvider.GetRequiredService<ICssClassAttributeResolver>()!.Resolve(this);
                CssClassBuilder.Append(result);

                this.BuildCssClass(CssClassBuilder);

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

        void BuildNavLink(Dictionary<string, object> htmlAttributes)
        {
            if (this is IHasNavLink navLink)
            {
                // Update computed state
                var href = (string?)null;
                if (htmlAttributes.TryGetValue("href", out var hrefAttribute))
                {
                    href = Convert.ToString(hrefAttribute);
                }

                _hrefAbsolute = href == null ? null : navLink.NavigationManager.ToAbsoluteUri(href).AbsoluteUri;
                IsNavLinkActived = ShouldNavLinkMatch(navLink.NavigationManager.Uri);
            }
        }

        void BuildForm(Dictionary<string, object> htmlAttributes)
        {
            if (this is IHasForm && _handleSubmitDelegate is not null)
            {
                htmlAttributes["onsubmit"] = _handleSubmitDelegate;
            }
        }
        #endregion
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
                    ((Task)propertyType!.GetMethod(nameof(AddChildComponent))!
                        .Invoke(propertyValue!, new[] { this }))!.GetAwaiter().GetResult();
                }
            }
        }
    }

    #region Disposable

    /// <summary>
    /// Releases the resouces in component.
    /// </summary>
    protected virtual void DisposeComponentResources() { }

    /// <summary>
    /// Releases unmanaged resouces in component.
    /// </summary>
    protected virtual void DisposeUnmanagedResouces() { }

    /// <summary>
    /// Disposes the component resources.
    /// </summary>
    /// <param name="disposing">If true, disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                DisposeComponentResources();

                if (this is IHasNavLink navLink)
                {
                    navLink.NavigationManager.LocationChanged -= OnNavLinkLocationChanged;
                }
            }

            DisposeUnmanagedResouces();

            disposedValue = true;
        }
    }

    /// <summary>
    /// Finalized component by GC.
    /// </summary>
    ~BlazorComponentBase()
    {
        Dispose(disposing: false);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion

    #endregion

    #region Private

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

            genericMethod.Invoke(null, new object[] { builder, this, 0, content, parentComponent.Name!, parentComponent.IsFixed });
        }

        void CreateComponentOrElement(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
        {
            var tagName = (TagName ?? GetElementTagName()) ?? throw new InvalidOperationException("Tag name cannot be null or empty");
            builder.OpenElement(0, tagName);
            continoues(builder);
            builder.CloseElement();
        }
    }


    #endregion
    #endregion

    #region NavLink

    /// <summary>
    /// Gets a bool value indicates current nav link is actived.
    /// </summary>
    protected bool IsNavLinkActived { get; private set; }
    string? _hrefAbsolute;

    /// <summary>
    /// Occurs when location of navigation is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnNavLinkLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // We could just re-render always, but for this component we know the
        // only relevant state change is to the _isActive property.
        var shouldBeActiveNow = ShouldNavLinkMatch(e.Location);
        if (shouldBeActiveNow != IsNavLinkActived)
        {
            IsNavLinkActived = shouldBeActiveNow;
            CssClassBuilder.Dispose();
            StateHasChanged();
        }
    }

    /// <summary>
    /// Shoulds the match.
    /// </summary>
    /// <param name="currentUriAbsolute">The current uri absolute.</param>
    /// <returns>A bool.</returns>
    private bool ShouldNavLinkMatch(string currentUriAbsolute)
    {
        if (this is not IHasNavLink navLink)
        {
            return false;
        }

        if (_hrefAbsolute == null)
        {
            return false;
        }

        if (EqualsHrefExactlyOrIfTrailingSlashAdded(currentUriAbsolute))
        {
            return true;
        }

        if (navLink.Match == NavLinkMatch.Prefix
            && IsStrictlyPrefixWithSeparator(currentUriAbsolute, _hrefAbsolute))
        {
            return true;
        }

        return false;


        bool EqualsHrefExactlyOrIfTrailingSlashAdded(string currentUriAbsolute)
        {
            Debug.Assert(_hrefAbsolute != null);

            if (string.Equals(currentUriAbsolute, _hrefAbsolute, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (currentUriAbsolute.Length == _hrefAbsolute.Length - 1)
            {
                // Special case: highlight links to http://host/path/ even if you're
                // at http://host/path (with no trailing slash)
                //
                // This is because the router accepts an absolute URI value of "same
                // as base URI but without trailing slash" as equivalent to "base URI",
                // which in turn is because it's common for servers to return the same page
                // for http://host/vdir as they do for host://host/vdir/ as it's no
                // good to display a blank page in that case.
                if (_hrefAbsolute[_hrefAbsolute.Length - 1] == '/'
                    && _hrefAbsolute.StartsWith(currentUriAbsolute, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        bool IsStrictlyPrefixWithSeparator(string value, string prefix)
        {
            var prefixLength = prefix.Length;
            if (value.Length > prefixLength)
            {
                return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
                    && (
                        // Only match when there's a separator character either at the end of the
                        // prefix or right after it.
                        // Example: "/abc" is treated as a prefix of "/abc/def" but not "/abcdef"
                        // Example: "/abc/" is treated as a prefix of "/abc/def" but not "/abcdef"
                        prefixLength == 0
                        || !char.IsLetterOrDigit(prefix[prefixLength - 1])
                        || !char.IsLetterOrDigit(value[prefixLength])
                    );
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region Form

    EditContext? _fixedEditContext;
    private readonly Func<Task>? _handleSubmitDelegate;

    /// <summary>
    /// Only provides for component implemented from <see cref="IHasForm"/> and initialize the form parameters.
    /// </summary>
    protected void OnFormParameterSet()
    {
        if (this is not IHasForm form)
        {
            return;
        }

        var _hasSetEditContextExplicitly = _fixedEditContext is not null;

        if (_hasSetEditContextExplicitly && form.Model is not null)
        {
            throw new InvalidOperationException($"{GetType().Name} required a {nameof(form.Model)} " +
                $"paremeter, or a {nameof(form.EditContext)} parameter, but not both.");
        }
        else if (!_hasSetEditContextExplicitly && form.Model is null)
        {
            throw new InvalidOperationException($"{GetType().Name} requires either a {nameof(form.Model)} parameter, or an {nameof(EditContext)} parameter, please provide one of these.");
        }

        if (form.OnSubmit.HasDelegate && (form.OnValidSubmit.HasDelegate || form.OnInvalidSubmit.HasDelegate))
        {
            throw new InvalidOperationException($"when supplying a {nameof(form.OnSubmit)} parameter to {GetType().Name}, do not also supply {nameof(form.OnValidSubmit)} or {nameof(form.OnInvalidSubmit)}.");
        }

        if (form.EditContext is not null && form.Model is null)
        {
            _fixedEditContext = form.EditContext;
        }
        else if (form.Model != null && form.Model != _fixedEditContext?.Model)
        {
            _fixedEditContext = new EditContext(form.Model!);
        }
    }

    /// <summary>
    /// Asynchorsouly submit current form component that implemented from <see cref="IHasForm"/> interface.
    /// </summary>
    /// <returns>A task contains validation result after task is completed.</returns>
    public async Task<bool> SubmitFormAsync()
    {
        if (this is not IHasForm form)
        {
            return false;
        }

        if (_fixedEditContext is null)
        {
            return false;
        }

        if (form.OnSubmit.HasDelegate)
        {
            await form.OnSubmit.InvokeAsync(_fixedEditContext);
            return false;
        }

        bool isValid = _fixedEditContext.Validate();
        if (isValid && form.OnValidSubmit.HasDelegate)
        {
            await form.OnValidSubmit.InvokeAsync(_fixedEditContext);
        }

        if (!isValid && form.OnInvalidSubmit.HasDelegate)
        {
            await form.OnInvalidSubmit.InvokeAsync(_fixedEditContext);
        }
        return isValid;
    }
    #endregion
}
