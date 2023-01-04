using ComponentBuilder.Abstrations.Internal;

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
        Resolvers = new List<IHtmlAttributesResolver>()
        {
            new HtmlAttributeAttributeResolver(),
            new HtmlDataAttributeResolver(),
            new HtmlEventAttributeResolver()
        };
    }

    /// <summary>
    /// Gets or sets the list of resolvers for HTML attributes for component.
    /// </summary>
    public IList<IHtmlAttributesResolver> Resolvers { get; internal set; } 
    /// <summary>
    /// Gets or sets the instance of <see cref="ICssClassBuilder"/>. <c>Null</c> to use default instance.
    /// </summary>
    public ICssClassBuilder? ClassBuilder { get; set; }

    /// <summary>
    /// Gets or sets the instance of <see cref="IStyleBuilder"/>. <c>Null</c> to use default instance.
    /// </summary>
    public IStyleBuilder? StyleBuilder { get; set; }
}
