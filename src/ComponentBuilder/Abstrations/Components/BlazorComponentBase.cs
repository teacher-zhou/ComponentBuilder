using System.Reflection;

using ComponentBuilder.Abstrations.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace ComponentBuilder;

/// <summary>
/// 表示具备框架特性的组件基类。
/// </summary>
public abstract class BlazorComponentBase : ComponentBase, IBlazorComponent, IRefreshableComponent
{
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
    /// Get instance of <see cref="IServiceProvider"/> .
    /// </summary>
    [Inject] protected IServiceProvider ServiceProvider { get; set; }

    #endregion Injection

    #region Parameters

    /// <summary>
    /// Gets or sets addtional attributes in element, it can automatically capture unmatched values of html attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Gets or sets the additional CSS class to append after <see cref="BuildCssClass(ICssClassBuilder)"/> method called.
    /// </summary>
    [Parameter] public string? AdditionalCssClass { get; set; }
    /// <summary>
    /// Gets or sets the additional style to append after <see cref="BuildStyle(IStyleBuilder)"/> method called.
    /// </summary>
    [Parameter] public string? AdditionalStyle { get; set; }
    /// <summary>
    /// Gets or set the extensions of CSS class utility built-in component.
    /// </summary>
    [Parameter] public ICssClassProvider CssClass { get; set; }

    #endregion Parameters

    #region Protected
    /// <summary>
    /// Gets instance of <see cref="IJSRuntime"/> after invoke lazy initialization.
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
    /// Gets or sets environment support WebAssembly of Server.
    /// </summary>
    /// <value><c>true</c> is WebAssembly, otherwise <c>false</c>.</value>
    protected Lazy<bool> IsWebAssembly => new(() => JS.Value is IJSInProcessRuntime);

    /// <summary>
    /// Gets instance of <see cref="ICssClassBuilder"/> .
    /// </summary>
    protected ICssClassBuilder CssClassBuilder { get; }
    /// <summary>
    /// Gets instance of <see cref="IStyleBuilder"/> .
    /// </summary>
    protected IStyleBuilder StyleBuilder { get; }
    /// <summary>
    /// Overrides to create HTML tag name.
    /// </summary>
    /// <exception cref="InvalidOperationException">Tag name is null, empty or whitespace.</exception>
    protected virtual string TagName
    {
        get
        {
            var tagName = ServiceProvider.GetRequiredService<HtmlTagAttributeResolver>().Resolve(this);
            if (string.IsNullOrWhiteSpace(tagName))
            {
                throw new InvalidOperationException($"The tag name is null, empty or whitespace.");
            }
            return tagName;
        }
    }

    /// <summary>
    /// Overrides to generate start sequence of source code in <see cref="RenderTreeBuilder"/> instance.
    /// </summary>
    protected virtual int RegionSequence => GetHashCode();

    /// <summary>
    /// Gets the collection of child component.
    /// </summary>
    public BlazorComponentCollection ChildComponents { get; private set; } = new();

    /// <summary>
    /// Gets or sets an action performed when child component is added.
    /// </summary>
    protected Action<IComponent>? OnComponentAdded { get; set; }
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

    #region Core
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    protected override void OnInitialized()
    {
        AddCascadingComponent();
        base.OnInitialized();
    }

    /// <summary>
    /// <inheritdoc/>
    /// <para>
    /// Note: DO NOT override this method unless you understand all features of component creation.
    /// </para>
    /// <para>
    /// Suggestion to override this method:
    /// </para>
    /// <para>
    /// Overrides <see cref="AddContent(RenderTreeBuilder, int)"/> to create specific inner content with minimize code.
    /// </para>
    /// <para>
    /// Overrides <see cref="BuildComponentRenderTree(RenderTreeBuilder)"/> to instead <see cref="BuildRenderTree(RenderTreeBuilder)"/> method if necessary.
    /// </para>
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
    /// Returns a string for CSS of this component built with features.
    /// </summary>
    /// <returns>A string separated by space for each item.</returns>
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
    /// Returns a string for style of this component build with features.
    /// </summary>
    /// <returns>A string separated by ';' for each item.</returns>
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
    /// Notify the state of component has been changed and re-render component.
    /// </summary>
    public Task NotifyStateChanged() => InvokeAsync(StateHasChanged);

    /// <summary>
    /// Add child component to this component.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="component"/> is null。</exception>
    public virtual Task AddChildComponent(IComponent component)
    {
        if (component is null)
        {
            throw new ArgumentNullException(nameof(component));
        }

        ChildComponents.Add(component);
        OnComponentAdded?.Invoke(component);
        return this.Refresh();
    }
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
    /// Build component common attributes and return the last sequence number of source code.
    /// <para>
    /// The order of building follow as below:
    /// </para>
    /// <list type="number">
    /// <item>
    /// Call <see cref="AddClassAttribute(RenderTreeBuilder, int)"/> method;
    /// </item>
    /// <item>
    /// Call <see cref="AddStyleAttribute(RenderTreeBuilder, int)"/> method;
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
    /// Instead of <see cref="BuildRenderTree(RenderTreeBuilder)"/> to build component tree.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> class.</param>
    protected virtual void BuildComponentRenderTree(RenderTreeBuilder builder)
    {
        BuildComponentAttributes(builder, out var sequence);
        AddContent(builder, sequence + 2);
    }
    #region AddContent
    /// <summary>
    /// Appends the content frame of component.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected virtual void AddContent(RenderTreeBuilder builder, int sequence) => AddChildContent(builder, sequence);

    /// <summary>
    /// Appends a child content of component that has implemented <see cref="IHasChildContent"/> instance automatically.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected void AddChildContent(RenderTreeBuilder builder, int sequence)
    {
        if (this is IHasChildContent content)
        {
            builder.AddContent(sequence, content.ChildContent);
        }
        //if (typeof(IHasChildContent<>).IsAssignableFrom(this.GetType()))
        //{
        //    var componentType = GetType();
        //    var childContentInterfaceType = componentType.GetInterfaces().First(m => m == typeof(IHasChildContent<>));


        //}
    }


    /// <summary>
    /// Appends a child content of component that has implemented <see cref="IHasChildContent{TValue}"/> instance automatically.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    /// <param name="value">The value of child content.</param>
    protected void AddChildContent<TValue>(RenderTreeBuilder builder, int sequence, TValue value)
    {
        if (this is IHasChildContent<TValue> content)
        {
            builder.AddContent(sequence, content.ChildContent, value);
        }
    }

    #endregion
    /// <summary>
    /// Appends a 'class' attribute to component if value of 'class' is not empty.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected void AddClassAttribute(RenderTreeBuilder builder, int sequence)
    {
        var cssClass = GetCssClassString();
        if (!string.IsNullOrEmpty(cssClass))
        {
            builder.AddAttribute(sequence, "class", cssClass);
        }
    }
    /// <summary>
    /// Appends a 'style' attribute to component if value of 'style' is not empty.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to append.</param>
    /// <param name="sequence">An integer that represents the position of the instruction in the source code.</param>
    protected void AddStyleAttribute(RenderTreeBuilder builder, int sequence)
    {
        var style = GetStyleString();
        if (!string.IsNullOrEmpty(style))
        {
            builder.AddAttribute(sequence, "style", style);
        }
    }

    /// <summary>
    /// Add a frame that represents multiple properties with the same sequence number including identification of specific indicators to create HTML attribute.
    /// </summary>
    /// <param name="builder">The instance of <see cref="RenderTreeBuilder"/> to append.</param>
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
            attributes = attributes.Concat(HtmlHelper.MergeHtmlAttributes(AdditionalAttributes));
        }

        BuildAttributes(AdditionalAttributes);
        attributes = attributes.Concat(AdditionalAttributes);

        builder.AddMultipleAttributes(sequence, attributes.Distinct());
    }

    /// <summary>
    /// Try get 'class' attribute from HTML element of this component.
    /// </summary>
    /// <param name="cssClass">The value of 'class' attribute，it may be <c>null</c>.</param>
    /// <returns><c>true</c> if 'class' attribute exists, otherwise, <c>false</c>.</returns>
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
    /// Try get 'style' attribute from HTML element of this component.
    /// </summary>
    /// <param name="style">The value of 'style' attribute，it may be <c>null</c>.</param>
    /// <returns><c>true</c> if 'style' attribute exists, otherwise, <c>false</c>.</returns>
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
    /// Add this component to parent compnent witch has identifies <see cref="ChildComponentAttribute"/> of this component that supply the cascading parameter of parent component.
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
                    throw new InvalidOperationException($"The value of property '{property.Name}' with type of '{propertyType.Name}' is null, which means component '{componentType.Name}' must be the child of component '{attr.ComponentType.Name}'. Set {nameof(ChildComponentAttribute.Optional)} is true for '{nameof(ChildComponentAttribute)}' in current component can solve this issue");
                }

                if (propertyType is not null && propertyValue is not null)
                {
                    ((Task)propertyType!.GetMethod(nameof(AddChildComponent))!
                        .Invoke(propertyValue, new[] { this })).GetAwaiter().GetResult();
                }
            }
        }
    }
    #endregion

    #region Private


    private void CreateComponentOrElement(RenderTreeBuilder builder, Action<RenderTreeBuilder> continoues)
    {

        var renderComponentAttribute = GetType().GetCustomAttribute<ComponentRenderAttribute>();

        var hasComponentAttr = renderComponentAttribute is not null;

        if (hasComponentAttr)
        {
            if (renderComponentAttribute!.ComponentType == GetType())
            {
                throw new InvalidOperationException($"Cannot create self component of {renderComponentAttribute.ComponentType.Name}");
            }
            builder.OpenComponent(0, renderComponentAttribute.ComponentType);
        }
        else
        {
            builder.OpenElement(0, TagName);
        }

        continoues(builder);

        if (hasComponentAttr)
        {
            builder.CloseComponent();
        }
        else
        {
            builder.CloseElement();
        }
    }
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

            genericMethod.Invoke(null, new object[] { builder, this, 0, content, parentComponent.Name, parentComponent.IsFixed });
        }
    }

    #endregion
    #endregion
}
