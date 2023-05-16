using ComponentBuilder.Automation.Builder;
using ComponentBuilder.Automation.Interceptors;
using ComponentBuilder.Automation.Rendering;

namespace ComponentBuilder.Automation;

/// <summary>
/// The options of ComponentBuilder.Automation.
/// </summary>
internal class ComponentBuilderOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentBuilderOptions"/> class.
    /// </summary>
    public ComponentBuilderOptions()
    {
        //Resolvers = new List<Type>()
        //{
        //    typeof(HtmlAttributeAttributeResolver),
        //    typeof(CssClassAttributeResolver),
        //    //typeof(FluentCssClassResolver),
        //};
        //Interceptors = new List<Type>()
        //{
        //    typeof(ChildContentInterceptor),
        //    typeof(AssociationComponentInterceptor),
        //    typeof(FormComponentInterceptor),
        //    typeof(CssClassAttributeInterceptor),
        //    typeof(StyleAttributeInterceptor),
        //};
        //Renderers = new List<Type>()
        //{
        //    typeof(NavLinkComponentRender),
        //};
    }
    ///// <summary>
    ///// Gets or sets the mode of debug. <c>True</c> to add <see cref="DebugInterceptor"/>.
    ///// </summary>
    //public bool Debug { get; set; }
    ///// <summary>
    ///// Gets the list of resolvers for HTML attributes from component parameters. The type of instance must implement from <see cref="IHtmlAttributeResolver"/> interface.
    ///// </summary>
    //public IList<Type> Resolvers { get; internal set; } = new List<Type>();
    ///// <summary>
    ///// Gets the list of interceptors for component attributes. The type of instance must implement from <see cref="IComponentInterceptor"/> interface.
    ///// </summary>
    //public IList<Type> Interceptors { get; internal set; }= new List<Type>();

    ///// <summary>
    ///// Gets the render of component. The type of instance must implement from <see cref="IComponentRender"/> interface.
    ///// </summary>
    //public IList<Type> Renderers { get; internal set; } = new List<Type>();

    /// <summary>
    /// Set <c>true</c> to capture element reference automatically. Default is <c>false</c>.
    /// <para>
    /// Set <see cref="BlazorComponentBase.CaptureReference"/> for isolation of current component.
    /// </para>
    /// </summary>
    public bool CaptureReference { get; set; }
}
