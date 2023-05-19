namespace ComponentBuilder.Resolvers;
/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class FluentClassComponentConfigurationBuilderExtensions
{
    /// <summary>
    /// Add a resolver that can support <see cref="FluentClass.IFluentClassProvider"/> for component parameters to build CSS class automatically.
    /// </summary>
    public static ComponentConfigurationBuilder AddFluentClassResolver(this ComponentConfigurationBuilder builder)
    => builder.AddResolver<IParameterClassResolver, FluentCssClassResolver>();
}
