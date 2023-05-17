namespace ComponentBuilder.Resolvers;
/// <summary>
/// The extensions of <see cref="ComponentConfigurationBuilder"/> class.
/// </summary>
public static class FluentClassComponentConfigurationBuilderExtensions
{

    public static ComponentConfigurationBuilder AddFluentClassResolver(this ComponentConfigurationBuilder builder)
    => builder.AddResolver<IParameterClassResolver, FluentCssClassResolver>();
}
