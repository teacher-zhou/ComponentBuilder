namespace ComponentBuilder.Resolvers;
/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class FluentClassComponentConfigurationBuilderExtensions
{
    /// <summary>
    /// Add the component parameters support <see href="IFluentClassProvider" /> build silky CSS parser.
    /// </summary>
    public static ComponentConfigurationBuilder AddFluentClassResolver(this ComponentConfigurationBuilder builder)
    => builder.AddResolver<IParameterClassResolver, FluentCssClassResolver>();
}
