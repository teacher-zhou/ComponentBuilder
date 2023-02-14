using ComponentBuilder.Builder;
using ComponentBuilder.Interceptors;
using ComponentBuilder.Rendering;
using ComponentBuilder.Resolvers;

namespace ComponentBuilder;

/// <summary>
/// The options of ComponentBuilder.
/// </summary>
public class ComponentBuilderOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentBuilderOptions"/> class.
    /// </summary>
    public ComponentBuilderOptions()
    {
        HtmlAttributeResolvers = new List<Type>()
        {
            typeof(HtmlAttributeAttributeResolver)
        };
        CssClassResolvers = new List<Type>()
        {
            typeof(CssClassAttributeResolver),
            typeof(FluentCssClassResolver),
        };
        Interceptors = new List<Type>()
        {
            typeof(ChildContentInterceptor),
            typeof(AssociationComponentInterceptor),
            typeof(FormComponentInterceptor),
            typeof(CssClassAttributeInterceptor),
            typeof(StyleAttributeInterceptor),
        };
        Renders = new List<Type>()
        {
            typeof(NavLinkComponentRender),
        };
        if (Debug)
        {
            Interceptors.Add(typeof(DebugInterceptor));
        }
    }
    /// <summary>
    /// Gets or sets the mode of debug. <c>True</c> to add <see cref="DebugInterceptor"/>.
    /// </summary>
    public bool Debug { get; set; }
    /// <summary>
    /// Gets the list of resolvers for HTML attributes from component parameters. The type of instance must implement from <see cref="IHtmlAttributeResolver"/> interface.
    /// </summary>
    public IList<Type> HtmlAttributeResolvers { get; internal set; }

    /// <summary>
    /// Gets the list of resolvers for class attribute from component parameters. The type of instance must implement from <see cref="ICssClassResolver"/> interface.
    /// </summary>
    public IList<Type> CssClassResolvers { get; internal set; }
    /// <summary>
    /// Gets the list of interceptors for component attributes. The type of instance must implement from <see cref="IComponentInterceptor"/> interface.
    /// </summary>
    public IList<Type> Interceptors { get; internal set; }

    /// <summary>
    /// Gets the render of component. The type of instance must implement from <see cref="IComponentRender"/> interface.
    /// </summary>
    public IList<Type> Renders { get; internal set; }

    /// <summary>
    /// Gets or sets the instance of <see cref="ICssClassBuilder"/>. <c>Null</c> to use default instance.
    /// </summary>
    public ICssClassBuilder? ClassBuilder { get; set; }

    /// <summary>
    /// Gets or sets the instance of <see cref="IStyleBuilder"/>. <c>Null</c> to use default instance.
    /// </summary>
    public IStyleBuilder? StyleBuilder { get; set; }

    /// <summary>
    /// Set <c>true</c> to capture element reference automatically. Default is <c>false</c>.
    /// <para>
    /// Set <see cref="BlazorComponentBase.CaptureReference"/> for isolation of current component.
    /// </para>
    /// </summary>
    public bool CaptureReference { get; set; }
}
