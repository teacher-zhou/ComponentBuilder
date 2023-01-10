using ComponentBuilder.Abstrations.Internal;
using ComponentBuilder.Interceptors;

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
        Resolvers = new List<IHtmlAttributeResolver>()
        {
            new HtmlAttributeAttributeResolver(),
            new HtmlDataAttributeResolver(),
            new HtmlEventAttributeResolver()
        };

        Interceptors = new List<IComponentInterceptor>()
        {
            new ChildContentInterceptor(),
            new AssociationComponentInterceptor(),
            new NavLinkComponentInterceptor(),
            new FormComponentInterceptor(),
            new CssClassAttributeInterceptor(),
            new StyleAttributeInterruptor(),
        };
    }

    /// <summary>
    /// Gets the list of resolvers for HTML attributes from component parameters.
    /// </summary>
    public IList<IHtmlAttributeResolver> Resolvers { get; internal set; }
    /// <summary>
    /// Gets the list of interceptors for component attributes.
    /// </summary>
    public IList<IComponentInterceptor> Interceptors { get; internal set; }
    /// <summary>
    /// Gets or sets the instance of <see cref="ICssClassBuilder"/>. <c>Null</c> to use default instance.
    /// </summary>
    public ICssClassBuilder? ClassBuilder { get; set; }

    /// <summary>
    /// Gets or sets the instance of <see cref="IStyleBuilder"/>. <c>Null</c> to use default instance.
    /// </summary>
    public IStyleBuilder? StyleBuilder { get; set; }

    /// <summary>
    /// Set <c>true</c> to capture element reference automatically. Default is <c>true</c>.
    /// <para>
    /// Set <see cref="BlazorComponentBase.CaptureReference"/> for isolation of current component.
    /// </para>
    /// </summary>
    public bool CaptureReference { get; set; } = true;
}
